using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.CloudParse;
using Newtonsoft.Json;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析系统 服务实现
    /// </summary>
    public class ParseSystemService : BaseKdyService, IParseSystemService
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;

        public ParseSystemService(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon)
            : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
        }

        /// <summary>
        /// 影片发送入库
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ParseVodSendAsync(ParseVodSendInput input)
        {
            var baseUrl = new Uri(input.ApiUrl);
            var reqInput = new KdyRequestCommonInput($"{baseUrl.Scheme}://{baseUrl.Host}")
            {
                TimeOut = 5000,
                UserAgent = "cloud parse agent",
            };
            dynamic tempData;
            switch (input.SendType)
            {
                case CmsSendType.Mac10:
                    {
                        tempData = BuildMac10PostData(input);
                        break;
                    }
                case CmsSendType.Self:
                    {
                        tempData = BuildSelfPostData(input);
                        break;
                    }
                default:
                    {
                        throw new KdyCustomException("未知类型");
                    }
            }

            reqInput.SetPostData(baseUrl.PathAndQuery, JsonConvert.SerializeObject(tempData));
            var sendResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (sendResult.IsSuccess)
            {
                return KdyResult.Success($"提交成功,CMS:{sendResult.Data}");
            }

            return KdyResult.Error(KdyResultCode.Error, $"异常,{sendResult.ErrMsg}");
        }

        /// <summary>
        /// 生成苹果V10入库
        /// </summary>
        /// <returns></returns>
        public dynamic BuildMac10PostData(ParseVodSendInput input)
        {
            var tempStr = string.Join("#", input.PlayUrl
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            dynamic dynamicDict = new System.Dynamic.ExpandoObject();
            dynamicDict.pass = input.SendPassWord;
            //dynamicDict.type_id = input.VodTypeName;
            dynamicDict.type_name = input.VodTypeName;
            dynamicDict.vod_name = input.VodName;
            dynamicDict.vod_play_from = input.PlayFrom;
            dynamicDict.vod_play_url = tempStr;
            return dynamicDict;
        }

        /// <summary>
        /// 生成自有入库
        /// </summary>
        /// <returns></returns>
        public dynamic BuildSelfPostData(ParseVodSendInput input)
        {
            dynamic dynamicDict = new System.Dynamic.ExpandoObject();
            dynamicDict.token = input.SendPassWord;
            dynamicDict.year = input.VodTypeName;
            dynamicDict.vodName = input.VodName;
            dynamicDict.vodPlayUrls = input.PlayUrl;
            return dynamicDict;
        }
    }
}
