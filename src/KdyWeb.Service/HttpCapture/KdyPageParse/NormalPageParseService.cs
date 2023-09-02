using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Utility;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 通用站点页面解析 服务实现
    /// </summary>
    public class NormalPageParseService : BaseKdyWebPageParseService, INormalPageParseService
    {
        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchConfigRepository;
        public NormalPageParseService(IKdyRequestClientCommon kdyRequestClientCommon, IKdyRepository<PageSearchConfig, long> pageSearchConfigRepository) : base(kdyRequestClientCommon)
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
                var endText = string.Empty;
                bool? isEnd = null;
                if (string.IsNullOrEmpty(BaseConfig.SearchConfig.EndXpath) == false)
                {
                    //完结
                    endText = item.SelectSingleNode(BaseConfig.SearchConfig.EndXpath)?.InnerText;
                }

                if (string.IsNullOrEmpty(endText) == false)
                {
                    isEnd = endText.Contains(BaseConfig.SearchConfig.NotEndKey) == false;
                }

                var detailUrl = item.GetAttributeValue("href", "");
                if (detailUrl.StartsWith("http") == false)
                {
                    detailUrl = $"{BaseConfig.BaseHost}{detailUrl}";
                }

                #region 图片处理
                var img = string.Empty;
                if (BaseConfig.SearchConfig.ImgAttr != null &&
                    BaseConfig.SearchConfig.ImgAttr.Any())
                {
                    foreach (var imgItem in BaseConfig.SearchConfig.ImgAttr)
                    {
                        img = item.GetAttributeValue(imgItem, "");
                        if (string.IsNullOrEmpty(img) == false)
                        {
                            break;
                        }
                    }
                }
                #endregion

                #region 名称处理
                var name = string.Empty;
                if (BaseConfig.SearchConfig.NameAttr != null &&
                    BaseConfig.SearchConfig.NameAttr.Any())
                {
                    foreach (var nameItem in BaseConfig.SearchConfig.NameAttr)
                    {
                        name = item.GetAttributeValue(nameItem, "");
                        if (string.IsNullOrEmpty(name) == false)
                        {
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = item.InnerText;
                }
                #endregion

                var resultItem = new KdyWebPageSearchOutItem(name.RemoveStrExt(" "), detailUrl)
                {
                    IsEnd = isEnd,
                    ImgUrl = img
                };

                result.Items.Add(resultItem);
            }

            return KdyResult.Success(result);
        }

        public override KdyResult<List<KdyWebPagePageOut>> DetailResultHandler(KdyWebPageSearchOutItem searchItem, KdyRequestCommonResult searchResult)
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
                    searchItem.ImgUrl = searchResult.Data.GetValueByXpath(BaseConfig.PageConfig.ImgXpath, tagItem);
                    if (string.IsNullOrEmpty(searchItem.ImgUrl) == false)
                    {
                        break;
                    }
                }
            }
            #endregion

            //详情页直接获取时 重新获取名称
            if (string.IsNullOrEmpty(searchItem.ResultName) &&
                string.IsNullOrEmpty(BaseConfig.PageConfig.NameXpath) == false)
            {
                searchItem.ResultName = searchResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.NameXpath)?.InnerText;
            }

            //详情完结
            if (searchItem.IsEnd == null &&
                string.IsNullOrEmpty(BaseConfig.PageConfig.EndXpath) == false)
            {
                var endText = searchResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.EndXpath)?.InnerText;
                searchItem.IsEnd = endText?.Contains(BaseConfig.SearchConfig.NotEndKey) == false;
            }

            //年份
            int year = -1;
            if (string.IsNullOrEmpty(BaseConfig.PageConfig.YearXpath) == false)
            {
                var yearText = searchResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.YearXpath)?.InnerText;
                int.TryParse(yearText, out year);
            }

            var md5 = hnc.First().ParentNode.ParentNode.InnerHtml.Md5Ext();
            foreach (var nodeItem in hnc)
            {
                var url = nodeItem.GetAttributeValue("href", "");
                var name = nodeItem.InnerText;
                if (url == "#" || url.Contains("javascript"))
                {
                    continue;
                }

                if (url.StartsWith("http") == false)
                {
                    url = $"{BaseConfig.BaseHost}{url}";
                }

                if (BaseConfig.BaseHost.Contains("360kan.com") == false)
                {
                    //360影视 除外
                    url = $"detail,{url}";
                }

                var pageOutItem = new KdyWebPagePageOut(md5, url, name)
                {
                    VideoYear = year
                };
                pageOutItem.ResultName = pageOutItem.ResultName.RemoveStrExt("\r", "\n", " ").GetNumber();
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
