using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi.Bilibili;
using KdyWeb.Dto.HttpApi.Steam;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.HttpApi;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace KdyWeb.Service.HttpApi
{
    /// <summary>
    /// B站 Api
    /// </summary>
    public class BilibiliHttpApi : BaseKdyService, IBilibiliHttpApi
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        public BilibiliHttpApi(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon) : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        /// <summary>
        /// 根据视频详情页获取视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<KdyResult<GetVideoInfoByDetailUrlResponse>> GetVideoInfoByDetailUrlAsync(GetVideoInfoByDetailUrlRequest input)
        {
            var request = new KdyRequestCommonInput(input.DetailUrl, HttpMethod.Get)
            {
                TimeOut = 5000,
                Referer = input.DetailUrl,
                EnCoding = null
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                if (response.HttpCode == HttpStatusCode.Forbidden)
                {
                    //请求被拦截
                    KdyLog.LogError($"分页Url:{input.DetailUrl}.请求被拦截");
                    return KdyResult.Error<GetVideoInfoByDetailUrlResponse>(KdyResultCode.Error, "请求被拦截");
                }

                KdyLog.LogWarning($"详情Url:{input.DetailUrl}异常.{response.ToJsonStr()}");
                return KdyResult.Error<GetVideoInfoByDetailUrlResponse>(KdyResultCode.Error, "请求异常");
            }

            var html = response.Data;
            var dataJson = html
                .Replace(" ", "")
                .GetStrMathExt("__INITIAL_STATE__=", ";\\(");
            var jObject = (JObject)JsonConvert.DeserializeObject(dataJson);
            var result = new GetVideoInfoByDetailUrlResponse()
            {
                AId = jObject.GetValueExt("aid"),
                BId = jObject.GetValueExt("bvid"),
                Title = jObject.GetValueExt("videoData.title"),
                Pic = jObject.GetValueExt("videoData.pic"),
                UpTime = jObject.GetValueExt("videoData.pubdate").ToInt32().ToDataTimeByTimestamp(),
                UpDataItem = new UpDataItem()
                {
                    Mid = jObject.GetValueExt("upData.mid"),
                    Name = jObject.GetValueExt("upData.name"),
                    Sex = jObject.GetValueExt("upData.sex"),
                    FansCount = jObject.GetValueExt("upData.fans").ToInt32(),
                    FriendCount = jObject.GetValueExt("upData.friend").ToInt32(),
                    Level = jObject.GetValueExt("upData.level_info.current_level").ToInt32(),
                    VipDueDate = jObject.GetValueExt("upData.vip.due_date").ToInt64().ToDataTimeByTimestamp(),
                },
                CountInfo = new GetVideoInfoByDetailUrlResponseCount()
                {
                    View = jObject.GetValueExt("videoData.stat.view").ToInt32(),
                    Coin = jObject.GetValueExt("videoData.stat.coin").ToInt32(),
                    Like = jObject.GetValueExt("videoData.stat.like").ToInt32(),
                    Favorite = jObject.GetValueExt("videoData.stat.favorite").ToInt32(),
                    Share = jObject.GetValueExt("videoData.stat.share").ToInt32(),
                    ElecCount = jObject.GetValueExt("elecFullInfo.total_count").ToInt32(),
                }
            };
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 根据弹幕用户Id获取用户Id
        /// </summary>
        /// <returns></returns>
        public string GetUidByMid(string userMid)
        {
            return null;
        }
    }
}
