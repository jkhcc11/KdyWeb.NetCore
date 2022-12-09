using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.Steam;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.HttpApi;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    ///  Steam WebApi
    /// </summary>
    public class SteamWebHttpApi : BaseKdyService, ISteamWebHttpApi
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        /// <summary>
        /// 国内CDN host
        /// </summary>
        private const string CdnHost = "https://media.st.dl.eccdnx.com";

        private readonly IConfiguration _configuration;
        public SteamWebHttpApi(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon,
            IConfiguration configuration) : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
            _configuration = configuration;
        }

        /// <summary>
        /// 根据商店Url获取游戏信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetGameInfoByStoreUrlResponse>> GetGameInfoByStoreUrlAsync(string storeUrl)
        {
            var ua = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.UaWithSteam)
                ?? "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36";
            var language = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.AcceptWithSteam)
                           ?? "zh-CN,zh;q=0.9";
            var cookie = _configuration.GetValue<string>(KdyWebServiceConst.KdyWebParseConfig.CookieWithSteam)
                         ?? "steamCountry=CN%7C5cf6c5b7111ba76637fc3b13636a3e07;browserid=3047278434067788844;timezoneOffset=28800,0;";
            var request = new KdyRequestCommonInput(storeUrl, HttpMethod.Get)
            {
                UserAgent = ua,
                TimeOut = 8,
                ExtData = new KdyRequestCommonExtInput()
                {
                    HeardDic = new Dictionary<string, string>()
                    {
                        {"Accept-Language",language}
                    }
                },
                Cookie = cookie
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                if (response.HttpCode == HttpStatusCode.Forbidden)
                {
                    //请求被拦截
                    KdyLog.LogError($"分页Url:{storeUrl}.请求被拦截");
                    return KdyResult.Error<GetGameInfoByStoreUrlResponse>(KdyResultCode.HttpError, "请求被拦截");
                }

                KdyLog.LogWarning($"详情Url:{storeUrl}异常.{response.ToJsonStr()}");
                return KdyResult.Error<GetGameInfoByStoreUrlResponse>(KdyResultCode.HttpError, "请求异常");
            }

            var html = response.Data;
            var movieNode = html.GetNodeCollection("//div[@class='highlight_strip_item highlight_strip_movie']");
            var movieList = new List<string>();
            //视频
            if (movieNode != null)
            {
                foreach (var nodeItem in movieNode)
                {
                    //id=thumb_movie_256919889
                    var movieId = nodeItem.Id.GetNumber();
                    movieList.Add($"{CdnHost}/steam/apps/{movieId}/movie_max.mp4");
                }
            }

            //截图
            var screenJsonStr = html.Replace(" ", "")
                .GetStrMathExt("rgScreenshotURLs=", ";");
            var screenList = new List<string>();
            if (screenJsonStr.IsEmptyExt() == false)
            {
                var screenArray = JsonConvert.DeserializeObject<Dictionary<string, string>>(screenJsonStr);
                foreach (var item in screenArray)
                {
                    screenList.Add(item.Value
                        .Replace("https://cdn.akamai.steamstatic.com", CdnHost)
                        .Replace("_SIZE_", ".600x338").Split('?').First());
                }
            }

            #region 语言支持判断
            var supportLanguage = new Dictionary<string, bool>();
            var languageNode = html.GetNodeCollection("//td[@class='ellipsis']");
            if (languageNode != null)
            {
                foreach (var item in languageNode)
                {
                    var languageStr = item.InnerText.Trim();

                    //判断
                    var uiNode = NextSiblingWithTd(item);
                    var uiText = uiNode.InnerText.Trim();
                    if (uiText == "不支持")
                    {
                        continue;
                    }

                    var isUi = uiText == "&#10004;";
                    var audioNode = NextSiblingWithTd(uiNode);
                    var isAudio = audioNode.InnerText.Trim() == "&#10004;";
                    var srtNode = NextSiblingWithTd(audioNode);
                    var isSrt = srtNode.InnerText.Trim() == "&#10004;";
                    switch (languageStr)
                    {
                        case "简体中文":
                            {
                                supportLanguage.Add(GetGameInfoByStoreUrlResponse.SupportLanguageConst.ZhUi, isUi);
                                supportLanguage.Add(GetGameInfoByStoreUrlResponse.SupportLanguageConst.ZhAudio, isAudio);
                                supportLanguage.Add(GetGameInfoByStoreUrlResponse.SupportLanguageConst.ZhSrt, isSrt);
                                break;
                            }
                        case "英语":
                            {
                                supportLanguage.Add(GetGameInfoByStoreUrlResponse.SupportLanguageConst.EnUi, isUi);
                                supportLanguage.Add(GetGameInfoByStoreUrlResponse.SupportLanguageConst.EnAudio, isAudio);
                                supportLanguage.Add(GetGameInfoByStoreUrlResponse.SupportLanguageConst.EnSrt, isSrt);
                                break;
                            }
                    }

                }
            }
            #endregion

            var result = new GetGameInfoByStoreUrlResponse()
            {
                GameName = html.GetValueByXpath("//div[@id='appHubAppName']", "text"),
                CovertUrl = html.GetValueByXpath("//img[@class='game_header_image_full']", "src").Split('?').First(),
                DetailUrl = html.GetValueByXpath("//meta[@property='og:url']", "content"),
                Description = html.GetValueByXpath("//div[@class='game_description_snippet']", "text"),
                MovieUrlList = movieList,
                ScreenshotList = screenList,
                SupperLanguage = supportLanguage
            };

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 同级td节点
        /// </summary>
        /// <returns></returns>
        private HtmlNode NextSiblingWithTd(HtmlNode node)
        {
            var tempNode = node.NextSibling;
            var i = 0;
            while (i < 5)
            {
                if (tempNode.Name != "td")
                {
                    tempNode = tempNode.NextSibling;
                    i++;
                    continue;
                }

                break;
            }

            return tempNode;
        }
    }
}
