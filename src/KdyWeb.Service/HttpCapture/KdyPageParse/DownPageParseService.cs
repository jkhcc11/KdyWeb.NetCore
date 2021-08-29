using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Hangfire.Dashboard;
using KdyWeb.BaseInterface;
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
    /// 下载站点页面解析 服务实现
    /// </summary>
    public class DownPageParseService : BaseKdyWebPageParseService, IDownPageParseService
    {

        /// <summary>
        /// 磁力前缀
        /// </summary>
        public const string MagnetPrefix = "magnet:?xt=";
        /// <summary>
        /// ED2K前缀
        /// </summary>
        public const string Ed2KPrefix = "ed2k://";

        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchConfigRepository;

        public DownPageParseService(IKdyRequestClientCommon kdyRequestClientCommon, IKdyRepository<PageSearchConfig, long> pageSearchConfigRepository) : base(kdyRequestClientCommon)
        {
            _pageSearchConfigRepository = pageSearchConfigRepository;
        }

        /// <summary>
        /// 下载站点屏蔽搜索
        /// </summary>
        /// <returns></returns>
        public override KdyResult<KdyWebPageSearchOut> SearchResultHandler(KdyRequestCommonResult searchResult)
        {
            var result = new KdyWebPageSearchOut()
            {
                Items = new List<KdyWebPageSearchOutItem>()
            };
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

            var year = YearHandler(searchResult);

            var panCloudKey = "baidu.com";
            var md5 = hnc.First().ParentNode.ParentNode.InnerHtml.Md5Ext();
            foreach (var nodeItem in hnc)
            {
                string nodeName = nodeItem.Name.ToLower(),
                    url = nodeItem.GetAttributeValue("href", ""),
                    name = nodeItem.InnerText.Trim();
                if (url.StartsWith(Ed2KPrefix) == false &&
                    url.StartsWith(MagnetPrefix) == false &&
                    url.Contains(panCloudKey) == false)
                {
                    continue;
                }

                if (nodeItem.Attributes.Contains("title"))
                {
                    name = nodeItem.GetAttributeValue("title", name);
                }

                if (nodeName == DownDetailNodeType.ANode.GetDisplayName().ToLower())
                {
                    var pageOutItem = new KdyWebPagePageOut(md5, url, name)
                    {
                        VideoYear = year
                    };

                    if (url.Contains(panCloudKey))
                    {
                        //百度网盘处理
                        //todo:看情况调整为数据库配置
                        var innerText = nodeItem.SelectSingleNode("../../td|../../../div[@class='post-content']")?.InnerText;
                        if (string.IsNullOrEmpty(innerText) == false)
                        {
                            //有些不用提取
                            pageOutItem.ResultUrl = BaiDuUrlHandler(innerText);
                            pageOutItem.ResultName = BaiDuUrlHandler(innerText);
                        }
                    }

                    ResultHandler(pageOutItem);
                    result.Add(pageOutItem);
                    continue;
                }

                throw new KdyCustomException($"下载详情页未实现节点名称：{nodeName},详情页：{searchOut.DetailUrl}");
            }

            if (result.Any() == false)
            {
                return KdyResult.Error<List<KdyWebPagePageOut>>(KdyResultCode.Error, "获取详情失败，无有效下载地址");
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
                IsEnd = true,
                ResultName = NameHandler(searchItem.ResultName),
                ImgUrl = ImgHandler(searchItem.ImgUrl),
                Results = detailResult.MapToListExt<NormalPageParseItem>()
            };

            ResultNameHandler(result);
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

        /// <summary>
        /// 结果处理
        /// </summary>
        private void ResultHandler(KdyWebPagePageOut result)
        {
            #region 链接处理
            if (result.ResultUrl.StartsWith(MagnetPrefix))
            {
                //磁力只要hash ([a-zA-z0-9]{32})
                var reg = new Regex(@"([a-zA-z0-9]{32})", RegexOptions.RightToLeft);
                var match = reg.Match(result.ResultUrl);
                result.ResultUrl = $"magnet:?xt=urn:btih:{match.Groups[1].Value.ToLower()}";
            }
            else if (result.ResultUrl.StartsWith(Ed2KPrefix))
            {
                var nameReg = new Regex(@"([【\[].*[】\]])", RegexOptions.RightToLeft);
                //var nameMatch = nameReg.Match(result.ResultUrl);
                result.ResultUrl = nameReg.Replace(result.ResultUrl, "");

                //用| 分割 大于6取 2、3、4、5 否则取 2、3、4
                var tempArray = result.ResultUrl.Split('|');
                var name = tempArray[2];
                if (name.Count(a => a == '%') > 2)
                {
                    //说明是url编码过的
                    name = HttpUtility.UrlDecode(name);
                }

                result.ResultName = name;
                if (tempArray.Length > 6)
                {
                    result.ResultUrl = $"ed2k://|file|{name}|{tempArray[3]}|{tempArray[4]}|{tempArray[5]}|/";
                }
                else
                {
                    result.ResultUrl = $"ed2k://|file|{name}|{tempArray[3]}|{tempArray[4]}|/";
                }
            }
            #endregion
        }

        /// <summary>
        /// 百度链接处理
        /// </summary>
        /// <returns></returns>
        private string BaiDuUrlHandler(string innerText)
        {
            var pwdReg = new Regex(@"(?<=\s*(密|提取)码[：:]?\s*)[A-Za-z0-9]+");
            var matchPwd = pwdReg.Match(innerText);

            var reg = new Regex(@"((?:https?:\/\/)?(?:yun|pan|eyun)\.baidu\.com\/(?:s\/\w*(((-)?\w*)*)?|share\/\S*\d\w*))");
            var match = reg.Match(innerText);

            return $"链接：{match.Value} 提取码：{matchPwd.Value}";
        }

        /// <summary>
        /// 年份处理
        /// </summary>
        /// <param name="detailResult">详情页请求结果</param>
        /// <returns></returns>
        private int YearHandler(KdyRequestCommonResult detailResult)
        {
            int year = -1;
            if (string.IsNullOrEmpty(BaseConfig.PageConfig.YearXpath))
            {
                return year;
            }

            var yearText = detailResult.Data.GetHtmlNodeByXpath(BaseConfig.PageConfig.YearXpath)?.InnerText;
            if (string.IsNullOrEmpty(yearText))
            {
                return year;
            }

            //直接正则提取
            var yearReg = new Regex("[0-9]{4}");
            var matchYear = yearReg.Match(yearText);
            int.TryParse(matchYear.Value, out year);
            if (year <= 1900)
            {
                //非年份
                return -2;
            }

            return year;
        }

        /// <summary>
        /// 影片名称处理
        /// </summary>
        private void ResultNameHandler(NormalPageParseOut result)
        {
            if (BaseConfig.BaseHost.StartsWith("http://n7f6.cn"))
            {
                var tempArray = result.ResultName.Split('(');
                result.ResultName = tempArray.First();
            }

            var resultNameReg = new Regex(@"[《(（]([\u4e00-\u9fa50-9A-Za-z/\\]*)[》)）]");
            var matchName = resultNameReg.Match(result.ResultName);
            if (matchName.Groups.Count >= 2)
            {
                result.ResultName = matchName.Groups[1].Value;
            }
        }
    }
}
