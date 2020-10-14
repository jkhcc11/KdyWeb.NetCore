using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 豆瓣站点相关 服务接口
    /// </summary>
    public class DouBanWebInfoService : BaseKdyService, IDouBanWebInfoService
    {
        /// <summary>
        /// 移动端Host
        /// </summary>
        private const string MobileHostUrl = "https://m.douban.com";
        /// <summary>
        /// PC端Host
        /// </summary>
        private const string PcHostUrl = "https://movie.douban.com";
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public DouBanWebInfoService(IKdyRequestClientCommon kdyRequestClientCommon)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        /// <summary>
        /// 根据豆瓣Id获取豆瓣信息
        /// https://m.douban.com/rexxar/api/v2/movie/35051512?ck=&for_mobile=1
        /// </summary>
        /// <remarks>
        ///  手机版模拟
        /// </remarks>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        public async Task<KdyResult<GetDouBanOut>> GetInfoBySubjectId(string subjectId)
        {
            if (subjectId.IsEmptyExt())
            {
                return KdyResult.Error<GetDouBanOut>(KdyResultCode.ParError, "豆瓣Id不能为空");
            }

            var reqResult = new KdyRequestCommonResult();

            #region 匹配正确Url
            var type = "movie";
            var subtype = Subtype.Movie;
            int i = 2;
            while (i-- > 0)
            {
                var reqInput = new KdyRequestCommonInput($"{MobileHostUrl}/rexxar/api/v2/{type}/{subjectId}?ck=&for_mobile=1", HttpMethod.Get)
                {
                    Referer = $"{MobileHostUrl}/movie/subject/{subjectId}/?event_source=movie_showing",
                    UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1",
                    ExtData = new KdyRequestCommonExtInput()
                    {
                        IsAjax = true
                    }
                };

                //请求豆瓣
                reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
                if (reqResult.IsSuccess == false)
                {
                    if (reqResult.HttpCode == HttpStatusCode.NotFound ||
                        reqResult.HttpCode == HttpStatusCode.Moved)
                    {
                        //默认是movie 不行就换tv
                        type = "tv";
                        subtype = Subtype.Tv;
                        continue;
                    }

                    //todo:这里有可能是403 就是需要cookie才能访问呢 后期加上
                }

                break;
            }
            #endregion

            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<GetDouBanOut>(KdyResultCode.Error, $"获取豆瓣信息异常，信息：{reqResult.ErrMsg}");
            }

            //成功肯定为json
            var tempJObject = JObject.Parse(reqResult.Data);
            var result = new GetDouBanOut(tempJObject.GetValueExt("title"), tempJObject.GetValueExt("year").ToInt32(),
                tempJObject.GetValueExt("pic.normal"), tempJObject.GetValueExt("id"))
            {
                Subtype = subtype,
                Actors = tempJObject.GetValueExt("actors", "name"),
                Directors = tempJObject.GetValueExt("directors", "name"),
                Tags = tempJObject.GetValueExt("genres"),
                Rating = tempJObject.GetValueExt("rating.value").ToDouble(),
                Intro = tempJObject.GetValueExt("intro"),
                Countries = tempJObject.GetValueExt("countries"),
                RatingsCount = tempJObject.GetValueExt("rating.count").ToInt32(),
                Aka = tempJObject.GetValueExt("aka"),
                CommentsCount = tempJObject.GetValueExt("comment_count").ToInt32(),
                ReviewsCount = tempJObject.GetValueExt("review_count").ToInt32()
            };

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 根据豆瓣Id获取豆瓣信息
        /// </summary>
        /// <remarks>
        ///  PC版模拟
        /// </remarks>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        public async Task<KdyResult<GetDouBanOut>> GetInfoBySubjectIdForPcWeb(string subjectId)
        {
            if (subjectId.IsEmptyExt())
            {
                return KdyResult.Error<GetDouBanOut>(KdyResultCode.ParError, "豆瓣Id不能为空");
            }

            var reqInput = new KdyRequestCommonInput($"{PcHostUrl}/subject/{subjectId}/", HttpMethod.Get)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.81 Safari/537.36 SE 2.X MetaSr 1.0"
            };

            //请求豆瓣
            var reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false || reqResult.Data.IsEmptyExt())
            {
                return KdyResult.Error<GetDouBanOut>(KdyResultCode.Error, $"获取豆瓣信息异常，信息：{reqResult.ErrMsg}");
            }

            //开始解析Html
            var year = reqResult.Data.GetValueByXpath("//*[@id='content']/h1/span[@class='year']", "text").RemoveStrExt("(", ")").ToInt32();
            var pic = reqResult.Data.GetValueByXpath("//*[@id='mainpic']/a/img", "src");
            var jsonStr = reqResult.Data.GetValueByXpath("//script[@type='application/ld+json']", "text");
            var tempJObject = JObject.Parse(jsonStr);
            var id = tempJObject.GetValueExt("url").RemoveStrExt("subject").Trim('/');
            var title = tempJObject.GetValueExt("name").Split(' ').First();
            Enum.TryParse(tempJObject.GetValueExt("@type").Replace("TVSeries","Tv"), out Subtype subtype);

            var result = new GetDouBanOut(title, year, pic, id)
            {
                Subtype = subtype,
                Actors = tempJObject.GetValueExt("actor", "name").HtmlPersonNameHandler(',',' '),
                Directors = tempJObject.GetValueExt("director", "name").HtmlPersonNameHandler(',', ' '),
                Tags = tempJObject.GetValueExt("genre"),
                Rating = tempJObject.GetValueExt("aggregateRating.ratingValue").ToDouble(),
                Intro = reqResult.Data.GetValueByXpath("//span[@property='v:summary']", "text"),
                RatingsCount = tempJObject.GetValueExt("aggregateRating.ratingCount").ToInt32(),
                CommentsCount = reqResult.Data.GetValueByXpath("//*[@id='comments-section']/div[1]/h2/span/a", "text").RemoveStrExt("全部", "条").ToInt32(),
                ReviewsCount = reqResult.Data.GetValueByXpath("//*[@id='reviews-wrapper']/header/h2/span/a", "text").RemoveStrExt("全部", "条").ToInt32(),
            };

            var tempHnc = reqResult.Data.GetNodeCollection("//*[@id='info']/span[@class='pl']");
            if (tempHnc == null)
            {
                return KdyResult.Success(result);
            }

            //遍历获取国家和又名
            foreach (var item in tempHnc)
            {
                var innerText = item.InnerText.InnerHtmlHandler();
                switch (innerText)
                {
                    case "制片国家/地区:":
                        {
                            result.Countries = item.NextSibling.InnerText.HtmlPersonNameHandler('/');
                            break;
                        }
                    case "又名:":
                        {
                            result.Aka = item.NextSibling.InnerText.InnerHtmlHandler().HtmlPersonNameHandler('/');
                            break;
                        }
                    case "IMDb链接:":
                        {
                            result.ImdbStr = item.NextSibling.NextSibling.GetAttributeValue("href", "");
                            break;
                        }
                }
            }


            return KdyResult.Success(result);
        }
    }
}
