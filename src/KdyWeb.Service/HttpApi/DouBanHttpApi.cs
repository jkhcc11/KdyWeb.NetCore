using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.HttpApi.DouBan;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpApi;
using KdyWeb.Utility;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    /// 豆瓣
    /// </summary>
    public class DouBanHttpApi : BaseKdyService, IDouBanHttpApi
    {
        private const string PcSearchHost = "https://movie.douban.com";
        private const string MobileWebHost = "https://m.douban.com";
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public DouBanHttpApi(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon) : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        /// <summary>
        /// 搜索提示
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        public async Task<KdyResult<List<SearchSuggestResponse>>> SearchSuggestAsync(string keyWord)
        {
            var request = new KdyRequestCommonInput($"{PcSearchHost}/j/subject_suggest?q={keyWord.ToUrlCodeExt().ToUpper()}", HttpMethod.Get)
            {
                TimeOut = 5000,
                Referer = PcSearchHost,
                ExtData = new KdyRequestCommonExtInput()
                {
                    IsAjax = true
                }
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, response.ErrMsg);
            }

            var resultArray = JArray.Parse(response.Data);
            if (resultArray.Any() == false)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, "暂无结果");
            }

            var result = new List<SearchSuggestResponse>();
            foreach (var token in resultArray)
            {
                var temp = new SearchSuggestResponse()
                {
                    Title = token.Value<string>("title"),
                    VideoImg = token.Value<string>("img"),
                    DouBanUrl = token.Value<string>("url"),
                    Subtitle = token.Value<string>("sub_title"),
                    Year = token.Value<int>("year"),
                    SubjectId = token.Value<string>("id")
                };
                temp.ImgHandler();
                result.Add(temp);
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 关键字搜索
        /// </summary>
        /// <remarks>
        /// 这个没有年份
        /// </remarks>
        /// <param name="keyWord">关键字</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        public async Task<KdyResult<List<SearchSuggestResponse>>> KeyWordSearchAsync(string keyWord, int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            page -= 1;
            var request = new KdyRequestCommonInput($"{MobileWebHost}/j/search/?q={keyWord.ToUrlCodeExt().ToUpper()}&t=1002&p={page}", HttpMethod.Get)
            {
                TimeOut = 5000,
                Referer = MobileWebHost,
                ExtData = new KdyRequestCommonExtInput(),
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.6 Mobile/15E148 Safari/604.1 Edg/119.0.0.0"
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, response.ErrMsg);
            }

            var jObject = JObject.Parse(response.Data);
            var tempHtml = jObject.Value<string>("html");
            if (string.IsNullOrEmpty(tempHtml))
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, "无内容");
            }

            var hnc = tempHtml.GetNodeCollection("//li/a");
            if (hnc.Any() == false)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, "html解析失败");
            }

            var result = new List<SearchSuggestResponse>();
            foreach (var nodeItem in hnc)
            {
                string href = nodeItem.GetAttributeValue("href", "");
                var temp = new SearchSuggestResponse()
                {
                    Title = nodeItem.SelectSingleNode("div[@class='subject-info']/span[@class='subject-title']").InnerText.Trim(),
                    VideoImg = nodeItem.SelectSingleNode("img").GetAttributeValue("src", ""),
                    DouBanUrl = $"{PcSearchHost}{nodeItem.GetAttributeValue("href", "").Replace("/movie", "")}",
                    SubjectId = href.GetNumber()
                };
                temp.ImgHandler();
                result.Add(temp);
            }

            return KdyResult.Success(result);
        }
    }
}
