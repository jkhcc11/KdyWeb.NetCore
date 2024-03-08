using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.HttpCapture.PageParse.CmsJson;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Utility;
using Newtonsoft.Json;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 资源站点json解析 服务实现
    /// </summary>
    public class ZyPageParseForJsonService : BaseKdyWebPageParseService, IZyPageParseForJsonService
    {
        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchConfigRepository;
        public ZyPageParseForJsonService(IKdyRequestClientCommon kdyRequestClientCommon, IKdyRepository<PageSearchConfig, long> pageSearchConfigRepository) : base(kdyRequestClientCommon)
        {
            _pageSearchConfigRepository = pageSearchConfigRepository;
        }

        public override KdyResult<KdyWebPageSearchOut> SearchResultHandler(KdyRequestCommonResult searchResult)
        {
            var result = new KdyWebPageSearchOut()
            {
                Items = new List<KdyWebPageSearchOutItem>()
            };

            //搜索结果
            //BaseConfig.SearchConfig.SearchXpath
            var cmsSearchResult =
                JsonConvert.DeserializeObject<BaseCmsNormalResultOut<CmsNormalWithSearchResultOut>>(searchResult.Data);
            if (cmsSearchResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyWebPageSearchOut>(KdyResultCode.Error, "搜索失败，无结果");
            }

            //BaseConfig.SearchConfig.NotEndKey
            result.Items = cmsSearchResult.ResultArray.Select(a => new KdyWebPageSearchOutItem(a.VodName, a.VodId)
            {
                IsEnd = a.IsEnd
            }).ToList();

            return KdyResult.Success(result);
        }

        public override KdyResult<List<KdyWebPagePageOut>> DetailResultHandler(KdyWebPageSearchOutItem searchOut, KdyRequestCommonResult searchResult)
        {
            var result = new List<KdyWebPagePageOut>();
            //详情页
            //BaseConfig.PageConfig.DetailXpath
            var cmsDetailList =
                JsonConvert.DeserializeObject<BaseCmsNormalResultOut<CmsNormalWithDetailOut>>(searchResult.Data);
            if (cmsDetailList.IsSuccess == false)
            {
                return KdyResult.Error<List<KdyWebPagePageOut>>(KdyResultCode.Error, "解析失败，获取详情Xpath失败");
            }

            var cmsDetail = cmsDetailList.ResultArray.First();
            searchOut.ImgUrl = cmsDetail.VodPic;
            searchOut.IsEnd = cmsDetail.IsEnd;
            searchOut.ResultName = cmsDetail.VodName;
            var md5 = cmsDetail.VodPlayUrl.Md5Ext();

            //设置默认后缀
            if (BaseConfig.PageConfig.PlayUrlSuffix == null ||
                BaseConfig.PageConfig.PlayUrlSuffix.Any() == false)
            {
                BaseConfig.PageConfig.PlayUrlSuffix = new[] { ".m3u8" };
            }

            //第01集$https://xxxxxx#第02集$https://xxxxxx$$$第01集$https://xxxxxx.m3u8#第02集$https://xxxxxx.m3u8
            //1、获取url分割标记
            //2、解析playUrl
            var playNote = cmsDetail.VodPlayNote;
            string[] tempPlayUrlArray;
            if (string.IsNullOrEmpty(playNote) == false)
            {
                //获取播放地址的多个通道
                tempPlayUrlArray = cmsDetail.VodPlayUrl.Split(playNote, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                //单通道
                tempPlayUrlArray = new[] { cmsDetail.VodPlayUrl };
            }

            if (tempPlayUrlArray.Any() == false)
            {
                return KdyResult.Error<List<KdyWebPagePageOut>>(KdyResultCode.Error, "解析失败，无播放地址");
            }

            //遍历通道只要符合后缀的通道
            var playIndex = -1;
            for (int i = 0; i < tempPlayUrlArray.Length; i++)
            {
                var playUrlItem = tempPlayUrlArray[i];
                var playUrl = playUrlItem.Split('#', StringSplitOptions.RemoveEmptyEntries).First();
                foreach (var suffixItem in BaseConfig.PageConfig.PlayUrlSuffix)
                {
                    if (playUrl.EndsWith(suffixItem) == false)
                    {

                        continue;
                    }

                    playIndex = i;
                    break;
                }
            }

            if (playIndex == -1)
            {
                return KdyResult.Error<List<KdyWebPagePageOut>>(KdyResultCode.Error, "解析失败，无有效播放地址");
            }

            //解析地址
            var lastPlayUrlItem = tempPlayUrlArray[playIndex];
            var lastPlayUrlArray = lastPlayUrlItem.Split('#', StringSplitOptions.RemoveEmptyEntries);
            foreach (var playItem in lastPlayUrlArray)
            {
                if (playItem.Contains("$") == false)
                {
                    continue;
                }

                var tempNameArray = playItem.Split('$');
                var pageOutItem = new KdyWebPagePageOut(md5, tempNameArray[1], tempNameArray[0])
                {
                    VideoYear = cmsDetail.VodYear
                };
                pageOutItem.ResultName = pageOutItem.ResultName.RemoveStrExt("\r", "\n", " ").GetNumber();
                pageOutItem.SpecialResultName();
                result.Add(pageOutItem);
            }

            return KdyResult.Success(result);
        }

        public override async Task<KdyResult<NormalPageParseOut>> KeyWordHandler(IList<KdyWebPageSearchOutItem> searchItems)
        {
            var firstData = searchItems.First();
            var detailResult = await GetPageResultAsync(firstData, new KdyWebPagePageInput()
            {
                DetailUrl = firstData.DetailUrl
            });
            if (detailResult.IsSuccess == false)
            {
                return KdyResult.Error<NormalPageParseOut>(KdyResultCode.Error, "解析失败，获取详情失败");
            }

            return DetailHandler(firstData, detailResult.Data);
        }

        public override KdyResult<NormalPageParseOut> DetailHandler(KdyWebPageSearchOutItem searchItem, List<KdyWebPagePageOut> detailResult)
        {
            //详情地址处理
            var detailUrl = $"{BaseConfig.BaseHost}{BaseConfig.PageConfig.DetailXpath}{searchItem.DetailUrl.GetNumber()}";
            var result = new NormalPageParseOut()
            {
                PageMd5 = detailResult.First().PageMd5,
                VideoYear = detailResult.First().VideoYear,
                DetailUrl = detailUrl,
                IsEnd = searchItem.IsEnd ?? false,
                ResultName = NameHandler(searchItem.ResultName),
                ImgUrl = ImgHandler(searchItem.ImgUrl),
                Results = detailResult.MapToListExt<NormalPageParseItem>()
            };
            return KdyResult.Success(result);
        }

        public override async Task<KdyResult<KdyRequestCommonResult>> SendDetailAsync(KdyWebPageSearchOutItem searchOut, KdyWebPagePageInput input)
        {
            //BaseConfig.PageConfig.DetailXpath 拼接详情地址
            var detailUrl = $"{BaseConfig.BaseHost}{BaseConfig.PageConfig.DetailXpath}{input.DetailUrl.GetNumber()}";
            var reqInput = new KdyRequestCommonInput(detailUrl, HttpMethod.Get)
            {
                UserAgent = BaseConfig.UserAgent,
                TimeOut = 3000
            };

            //请求
            var reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyRequestCommonResult>(KdyResultCode.Error, $"解析失败，获取详情失败.{reqResult.ErrMsg}");
            }

            return KdyResult.Success(reqResult);
        }

        protected override async Task<NormalPageParseConfig> GetConfigAsync(long configId)
        {
            var dbConfig = await _pageSearchConfigRepository.FirstOrDefaultAsync(a => a.Id == configId);
            if (dbConfig == null)
            {
                throw new NullReferenceException($"搜索配置：{configId} 未找到配置");
            }

            return dbConfig.ToNormalPageParseConfig();
        }
    }
}
