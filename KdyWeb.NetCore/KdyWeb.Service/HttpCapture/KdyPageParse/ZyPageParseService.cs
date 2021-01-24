using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 资源站点页面解析 服务实现
    /// </summary>
    public class ZyPageParseService : BaseKdyWebPageParseService, IZyPageParseService
    {
        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchConfigRepository;
        public ZyPageParseService(IKdyRequestClientCommon kdyRequestClientCommon, IKdyRepository<PageSearchConfig, long> pageSearchConfigRepository) : base(kdyRequestClientCommon)
        {
            _pageSearchConfigRepository = pageSearchConfigRepository;
        }

        public override KdyResult<KdyWebPageSearchOut> SearchResultHandler(KdyRequestCommonResult searchResult)
        {
            var result = new KdyWebPageSearchOut()
            {
                Items = new List<KdyWebPageSearchOutItem>()
            };
            //开始解析搜索结果
            var hnc = searchResult.Data.GetNodeCollection(BaseConfig.SearchConfig.SearchXpath);
            if (hnc == null || hnc.Count <= 0)
            {
                return KdyResult.Error<KdyWebPageSearchOut>(KdyResultCode.Error, "解析失败，搜索解析Xpath失败");
            }

            foreach (var item in hnc)
            {
                var detailUrl = item.GetAttributeValue("href", "");
                if (detailUrl.StartsWith("http") == false)
                {
                    detailUrl = $"{BaseConfig.BaseHost}{detailUrl}";
                }

                var name = item.SelectSingleNode("./text()")?.InnerText;
                var endText = item.SelectSingleNode(BaseConfig.SearchConfig.EndXpath)?.InnerText;

                var resultItem = new KdyWebPageSearchOutItem(name.RemoveStrExt(" "), detailUrl)
                {
                    IsEnd = endText?.Contains(BaseConfig.SearchConfig.NotEndKey) == false
                };

                result.Items.Add(resultItem);
            }

            return KdyResult.Success(result);
        }

        public override KdyResult<List<KdyWebPagePageOut>> DetailResultHandler(KdyWebPageSearchOutItem searchOut, KdyRequestCommonResult searchResult)
        {
            var result = new List<KdyWebPagePageOut>();

            //开始解析详情页
            var hnc = searchResult.Data.GetNodeCollection(BaseConfig.PageConfig.DetailXpath);
            if (hnc == null || hnc.Count <= 0)
            {
                return KdyResult.Error<List<KdyWebPagePageOut>>(KdyResultCode.Error, "解析失败，获取详情Xpath失败");
            }

            #region 图片处理
            if (string.IsNullOrEmpty(BaseConfig.PageConfig.ImgXpath) == false)
            {
                var imgTag = new[]
                {
                    "src",
                    "style"
                };
                foreach (var tagItem in imgTag)
                {
                    searchOut.ImgUrl = searchResult.Data.GetValueByXpath(BaseConfig.PageConfig.ImgXpath, tagItem);
                    if (string.IsNullOrEmpty(searchOut.ImgUrl) == false)
                    {
                        break;
                    }
                }
            }
            #endregion

            //详情页直接获取时 重新获取名称
            if (string.IsNullOrEmpty(searchOut.ResultName) &&
                string.IsNullOrEmpty(BaseConfig.PageConfig.NameXpath) == false)
            {
                searchOut.ResultName = searchResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.NameXpath)?.InnerText;
            }

            //详情完结
            if (searchOut.IsEnd == null &&
                string.IsNullOrEmpty(BaseConfig.PageConfig.EndXpath) == false)
            {
                var endText = searchResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.EndXpath)?.InnerText;
                searchOut.IsEnd = endText?.Contains(BaseConfig.SearchConfig.NotEndKey) == false;
            }

            //年份
            int year = -1;
            if (string.IsNullOrEmpty(BaseConfig.PageConfig.YearXpath) == false)
            {
                var yearText = searchResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.YearXpath)?.InnerText;
                int.TryParse(yearText, out year);
            }

            var md5 = hnc.First().ParentNode.ParentNode.InnerHtml.Md5Ext();

            //设置默认后缀
            if (BaseConfig.PageConfig.PlayUrlSuffix == null ||
                BaseConfig.PageConfig.PlayUrlSuffix.Any() == false)
            {
                BaseConfig.PageConfig.PlayUrlSuffix = new[] { ".m3u8" };
            }

            //获取优先匹配的后缀
            var prioritySuffix = string.Empty;
            foreach (var nodeItem in hnc)
            {
                var url = string.Empty;
                var name = string.Empty;
                var tempText = nodeItem.InnerText;
                if (tempText.Contains("$"))
                {
                    var tempArray = tempText.Split('$');
                    name = tempArray[0];
                    url = tempArray[1];
                }

                //优先匹配给定后缀 没有则直接跳过 有则后面以此为标准
                if (string.IsNullOrEmpty(prioritySuffix))
                {
                    foreach (var suffixItem in BaseConfig.PageConfig.PlayUrlSuffix)
                    {
                        if (url.EndsWith(suffixItem) == false)
                        {
                            continue;
                        }

                        prioritySuffix = suffixItem;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(prioritySuffix) ||
                    url.EndsWith(prioritySuffix) == false)
                {
                    //未匹配到后缀 或者当前url不是后缀
                    continue;
                }

                var pageOutItem = new KdyWebPagePageOut(md5, url, name)
                {
                    VideoYear = year
                };
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
            var result = new NormalPageParseOut()
            {
                PageMd5 = detailResult.First().PageMd5,
                VideoYear = detailResult.First().VideoYear,
                DetailUrl = searchItem.DetailUrl,
                IsEnd = searchItem.IsEnd ?? false,
                ResultName = NameHandler(searchItem.ResultName),
                ImgUrl = ImgHandler(searchItem.ImgUrl),
                Results = detailResult.MapToListExt<NormalPageParseItem>()
            };
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 重写详情请求
        /// </summary>
        /// <param name="searchOut"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<KdyResult<KdyRequestCommonResult>> SendDetailAsync(KdyWebPageSearchOutItem searchOut, KdyWebPagePageInput input)
        {
            if (input.DetailUrl.Contains("ac=videolist"))
            {
                //api地址 转详情
                var detailId = input.DetailUrl.GetNumber();
                input.DetailUrl = $"{BaseConfig.BaseHost}/?m=vod-detail-id-{detailId}.html";
            }

            var reqInput = new KdyRequestCommonInput(input.DetailUrl, HttpMethod.Get)
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
