using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.HttpApi.DouBan;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpApi;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    /// 豆瓣
    /// </summary>
    public class DouBanHttpApi : BaseKdyService, IDouBanHttpApi
    {
        private const string PcBaseHost = "https://www.douban.com";
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
            //不要这个 没有年份 不好自动匹配
            //https://www.douban.com/search?cat=1002&q=%E6%96%B0%E7%99%BD%E5%A8%98%E5%AD%90
            if (page <= 0)
            {
                page = 1;
            }

            page -= 1;
            var request = new KdyRequestCommonInput($"{PcBaseHost}/j/search?q={keyWord.ToUrlCodeExt().ToUpper()}&cat=1002&start={page * 20}", HttpMethod.Get)
            {
                TimeOut = 5000,
                Referer = PcBaseHost,
                ExtData = new KdyRequestCommonExtInput(),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36"
            };
            var cookie = KdyConfiguration.GetValue<string>(KdyWebServiceConst.DouBanCookieKey);
            if (string.IsNullOrEmpty(cookie) == false)
            {
                request.Cookie = cookie;
            }

            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, response.ErrMsg);
            }

            var jObject = JObject.Parse(response.Data);
            var code = jObject.Value<int>("code");
            if (code > 0)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, "无内容");
            }

            var jArray = jObject.Value<JArray>("items");
            if (jArray.Any() == false)
            {
                return KdyResult.Error<List<SearchSuggestResponse>>(KdyResultCode.Error, "html解析失败");
            }

            var result = new List<SearchSuggestResponse>();
            foreach (var jItem in jArray)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(jItem.Value<string>());

                var subjectCastContent = doc.DocumentNode.SelectSingleNode("//span[@class='subject-cast']").InnerText;
                var yearMatch = Regex.Match(subjectCastContent, @"\d{4}");
                var year = yearMatch.Success ? yearMatch.Value : null;

                var aHref = doc.DocumentNode.SelectSingleNode("//div[@class='title']/h3/a").GetAttributeValue("href", "");
                var idMatch = Regex.Match(aHref, @"subject%2F(\d+)");
                var subjectId = idMatch.Success ? idMatch.Groups[1].Value : null;

                var temp = new SearchSuggestResponse()
                {
                    Title = doc.DocumentNode.SelectSingleNode("//div[@class='title']/h3//a").InnerText.Trim(),
                    VideoImg = doc.DocumentNode.SelectSingleNode("//div[@class='pic']/a/img").GetAttributeValue("src", ""),
                    Year = year?.ToInt32() ?? 0,
                    DouBanUrl = $"{PcSearchHost}/subject/{subjectId}/",
                    SubjectId = subjectId,
                    Subtitle = doc.DocumentNode.SelectSingleNode("//div[@class='content']/p").InnerText.Trim()
                };
                temp.ImgHandler();
                result.Add(temp);
            }

            return KdyResult.Success(result);
        }
    }
}
