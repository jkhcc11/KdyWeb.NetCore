using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
    /// 磁力站点页面解析 服务实现
    /// </summary>
    public class TorrentPageParseService : BaseKdyWebPageParseService, ITorrentPageParseService
    {
        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchConfigRepository;
        public TorrentPageParseService(IKdyRequestClientCommon kdyRequestClientCommon, IKdyRepository<PageSearchConfig, long> pageSearchConfigRepository) : base(kdyRequestClientCommon)
        {
            _pageSearchConfigRepository = pageSearchConfigRepository;
        }

        /// <summary>
        /// 搜索结果处理
        /// </summary>
        /// <returns></returns>
        public override KdyResult<KdyWebPageSearchOut> SearchResultHandler(KdyRequestCommonResult searchResult)
        {
            var result = new KdyWebPageSearchOut()
            {
                Items = new List<KdyWebPageSearchOutItem>()
            };
            //开始解析搜索结果
            var hnc = searchResult.Data.GetNodeCollection(BaseConfig.SearchConfig.SearchXpath);
            if (hnc == null ||
                hnc.Any() == false)
            {
                return KdyResult.Error<KdyWebPageSearchOut>(KdyResultCode.Error, "解析失败，搜索解析Xpath失败");
            }

            foreach (var item in hnc)
            {
                var detailUrl = item.GetAttributeValue("href", "");
                var name = item.GetAttributeValue("title", "");
                name = name.IsEmptyExt() ?
                    item.InnerText : name;

                if (detailUrl.StartsWith("magnet:"))
                {
                    var temp = GetMagnetAndName(detailUrl);

                    //直接磁力 提取名称
                    name = temp.name;
                    detailUrl = temp.magnetLink;
                }
                else if (detailUrl.StartsWith("http") == false)
                {
                    detailUrl = $"{BaseConfig.BaseHost}{detailUrl}";
                }

                var resultItem = new KdyWebPageSearchOutItem(name?.RemoveStrExt(" "), detailUrl)
                {
                    IsEnd = false
                };

                result.Items.Add(resultItem);
            }

            return KdyResult.Success(result);
        }


        /// <summary>
        /// 关键字结果处理
        /// </summary>
        /// <param name="searchItems">搜索结果列表</param>
        /// <returns></returns>
        public override async Task<KdyResult<NormalPageParseOut>> KeyWordHandler(IList<KdyWebPageSearchOutItem> searchItems)
        {
            await Task.CompletedTask;
            var result = new NormalPageParseOut()
            {
                Results = new List<NormalPageParseItem>()
            };
            //磁力只要hash
            var reg = new Regex(@"([a-zA-Z0-9]{40}|[a-zA-Z0-9]{32})", RegexOptions.RightToLeft);
            foreach (var item in searchItems)
            {
                var tempMagnet = item.DetailUrl;
                if (item.DetailUrl.StartsWith("magnet:") == false)
                {
                    var match = reg.Match(item.DetailUrl);
                    if (match.Success == false)
                    {
                        //未匹配到 跳过
                        continue;
                    }

                    tempMagnet = $"magnet:?xt=urn:btih:{match.Groups[1].Value.ToLower()}";
                }

                result.Results.Add(new NormalPageParseItem()
                {
                    ResultName = item.ResultName,
                    ResultUrl = tempMagnet
                });
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 详情页处理
        /// </summary>
        /// <returns></returns>
        public override KdyResult<List<KdyWebPagePageOut>> DetailResultHandler(KdyWebPageSearchOutItem searchOut,
            KdyRequestCommonResult searchResult)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// 详情结果处理
        /// </summary>
        /// <param name="searchItem">详情信息</param>
        /// <param name="detailResult">详情结果列表</param>
        /// <returns></returns>
        public override KdyResult<NormalPageParseOut> DetailHandler(KdyWebPageSearchOutItem searchItem, List<KdyWebPagePageOut> detailResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取页面解析结果
        /// </summary>
        /// <returns></returns>
        public override Task<KdyResult<List<KdyWebPagePageOut>>> GetPageResultAsync(KdyWebPageSearchOutItem searchOut, KdyWebPagePageInput input)
        {
            throw new NotImplementedException();
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
        /// 获取磁力和名称  根据磁力完整链接 提取
        /// </summary>
        /// <returns></returns>
        private (string magnetLink, string name) GetMagnetAndName(string magnetLink)
        {
            var magnetMatch = Regex.Match(magnetLink, @"magnet:\?xt=urn:btih:([^&]+)");
            var dnMatch = Regex.Match(magnetLink, @"dn=([^&]+)");

            string torrentMagnetLink = string.Empty,
                name = string.Empty;
            if (magnetMatch.Success)
            {
                torrentMagnetLink = $"magnet:?xt=urn:btih:{magnetMatch.Groups[1].Value.ToLower()}";
            }

            if (dnMatch.Success)
            {
                name = Uri.UnescapeDataString(dnMatch.Groups[1].Value);
            }

            return (torrentMagnetLink, name);
        }
    }
}
