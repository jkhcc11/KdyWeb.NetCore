using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Utility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 腾讯文档WebApi 服务实现
    /// </summary>
    public class TxDocWebService : BaseKdyService, ITxDocWebService
    {
        private const string TxDocBaseUrl = "https://docs.qq.com";
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        private readonly TxDocRecordsOption _txDocRecordsOption;

        public TxDocWebService(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon,
            IOptions<TxDocRecordsOption> options) : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
            _txDocRecordsOption = options.Value;
        }

        /// <summary>
        /// 获取腾讯文档接龙列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetSequenceRecordsOut>> GetSequenceRecordsAsync(GetSequenceRecordsInput input)
        {
            if (input.DocUrl.IsEmptyExt() ||
                input.UserLoginCookie.IsEmptyExt())
            {
                return KdyResult.Error<GetSequenceRecordsOut>(KdyResultCode.ParError, "Url或登录信息为空");
            }

            var reqInput = new KdyRequestCommonInput(input.DocUrl, HttpMethod.Get)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36 Edg/135.0.0.0",
                ExtData = new KdyRequestCommonExtInput()
                {
                    IsAjax = false
                },
                Referer = input.DocUrl
            };
            var reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (string.IsNullOrEmpty(reqResult.Data))
            {
                return KdyResult.Error<GetSequenceRecordsOut>(KdyResultCode.Error, "获取内容无效");
            }

            //(escape(atob(' 或者 这个中间的base64
            var base64Data = reqResult.Data.GetStrMathExt("atob\\('", "'\\)");
            var decodeData = base64Data.Base64ToStrExt(Encoding.UTF8);
            var jObject = JObject.Parse(decodeData);

            var createUserId = jObject.GetValueExt("docInfo.createInfo.creatorId");
            var createUserNickName = jObject.GetValueExt("docInfo.createInfo.creatorNick");
            var docTitle = jObject.GetValueExt("docInfo.padInfo.padTitle");

            //文档Id  用来获取记录列表
            var padId = jObject.GetValueExt("padData.padId");
            if (string.IsNullOrEmpty(padId))
            {
                return KdyResult.Error<GetSequenceRecordsOut>(KdyResultCode.Error, "文档疑似被删除");
            }

            //根据文档Id获取接龙记录 需要使用cookie
            var reqData = new
            {
                form_id = padId,
                self_only = false,
                need_answers = true,
                filter_del = true,
                size = 100, //分页大小
                descending_order = false,
                order_by = 0,
                record_status = 0,
                user_type_filter = 0
            };
            var recordApiUrl = $"{TxDocBaseUrl}/api/form/read/records";
            reqInput.Cookie = input.UserLoginCookie;
            reqInput.SetPostData(recordApiUrl,
                reqData.ToJsonStr(),
                isAjax: true);

            //获取接龙记录
            reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (string.IsNullOrEmpty(reqResult.Data))
            {
                return KdyResult.Error<GetSequenceRecordsOut>(KdyResultCode.Error, "获取记录失败");
            }

            //{
            // "retcode": 0,
            // "msg": "成功",
            // "result": {
            //     "records": [
            //         {
            //             "seq": 3,
            //             "uid": "144115216523067739",
            //             "create_time": "2025-05-28T03:17:08Z",
            //             "pre_post": false,
            //             "record_status": 0,
            //             "base_seq": 0,
            //             "hidden_ids": [],
            //             "answers": "[{\"type\":\"SEQUENCE\",\"content\":\"\",\"dataType\":1,\"id\":\"sequence-blank-1\"}]",
            //             "role": {
            //                 "type": 0,
            //                 "name": "余",
            //                 "identify": "",
            //                 "birthday": "",
            //                 "role_id": "",
            //                 "relation": 0,
            //                 "member": null,
            //                 "user_info": {
            //                     "type": 0,
            //                     "nick": "余",
            //                     "avatar": "https://docsavatar.gtimg.com/dcb8c6d3a590896cb2758649e2265bdd62bb1de3/1667287528"
            //                 },
            //                 "extra_json": null
            //             },
            //             "paper_result": null,
            //             "qqzy_openid": "",
            //             "audit_status": 0,
            //             "del_info": null,
            //             "my_record": false
            //         }]}
            //
            var jRecodeObject = JObject.Parse(reqResult.Data);
            var statusMsg = jRecodeObject.GetValueExt("msg");
            if (statusMsg != "成功")
            {
                return KdyResult.Error<GetSequenceRecordsOut>(KdyResultCode.Error, $"获取记录失败,{statusMsg}");
            }

            var records = jRecodeObject["result"]?["records"] as JArray;
            if (records == null ||
                records.Count <= 0)
            {
                return KdyResult.Error<GetSequenceRecordsOut>(KdyResultCode.Error, "获取记录失败,无有效记录");
            }

            var result = new GetSequenceRecordsOut()
            {
                CreateUserId = createUserId,
                CreateUserNick = createUserNickName,
                DocTitle = docTitle,
            };
            foreach (var item in records)
            {
                result.SequenceRecordItems.Add(new SequenceRecordItem()
                {
                    NickName = item["role"]?["user_info"]?.Value<string>("nick"),
                    UId = item.Value<string>("uid"),
                    CreateTime = item.Value<DateTime>("create_time").AddHours(8),
                    ShowName = item["role"]?.Value<string>("name"),
                });
            }

            //SequenceRecordHandler(result.SequenceRecordItems);
            //ResultShowTextFormat(result);
            return KdyResult.Success(result);
        }

        ///// <summary>
        ///// 接龙记录处理
        ///// </summary>
        //private void SequenceRecordHandler(List<SequenceRecordItem> sequenceRecordItems)
        //{
        //    var defaultPrice = _txDocRecordsOption.MaxPrice;
        //    if (sequenceRecordItems.Count >= _txDocRecordsOption.NumberSteps)
        //    {
        //        defaultPrice = _txDocRecordsOption.MinPrice;
        //    }

        //    //遍历接龙人数 开始计算价格
        //    //有个去重，一个人当前接龙的奖励只有一次，有些是奖励人待接龙的
        //    foreach (var item in sequenceRecordItems)
        //    {
        //        var current = defaultPrice;

        //        var giftItem = _txDocRecordsOption.GiftUserItems.FirstOrDefault(a => a.UserId == item.UId);
        //        if (giftItem == null)
        //        {
        //            item.SetGiftUserType(GiftUserTypeEnum.Normal, current);
        //            continue;
        //        }

        //        if (sequenceRecordItems.Any(a => a.UId == giftItem.UserId &&
        //                                         a.GiftUserType != GiftUserTypeEnum.Normal))
        //        {
        //            //多次的 只有一次有效
        //            item.SetGiftUserType(GiftUserTypeEnum.Normal, current);
        //            continue;
        //        }


        //        switch (giftItem.GiftUserType)
        //        {

        //            case GiftUserTypeEnum.TimePrice:
        //                {
        //                    current = giftItem.GiftPrice;
        //                    break;
        //                }
        //            case GiftUserTypeEnum.VipUser:
        //                {
        //                    //todo:这里多名额的再说
        //                    if (sequenceRecordItems.Count >= _txDocRecordsOption.VipFreeSteps)
        //                    {
        //                        current = 0;
        //                        item.SetGiftUserType(giftItem.GiftUserType, current);
        //                    }
        //                    else
        //                    {
        //                        //原价
        //                        item.SetGiftUserType(GiftUserTypeEnum.Normal, current);
        //                    }
        //                    break;
        //                }
        //            case GiftUserTypeEnum.Card:
        //            case GiftUserTypeEnum.Free:
        //                {
        //                    current = 0;
        //                    break;
        //                }
        //        }

        //        if (giftItem.GiftUserType != GiftUserTypeEnum.VipUser)
        //        {
        //            item.SetGiftUserType(giftItem.GiftUserType, current);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 文案格式化
        ///// </summary>
        //private void ResultShowTextFormat(GetSequenceRecordsOut result)
        //{
        //    //文案格式化   总：xx  x付款+x扣卡（谁）
        //    //                 其中 x个xx    x个xx

        //    //扣卡价格
        //    var cardList = result.SequenceRecordItems
        //        .Where(a => a.GiftUserType == GiftUserTypeEnum.Card)
        //        .ToList();
        //    var cardStr =
        //        cardList.Any() ?
        //            $"+{cardList.Count}扣卡（{string.Join("、", cardList.Select(a => a.ShowName))}）" : "";

        //    //时间段价格
        //    var giftList = result.SequenceRecordItems
        //        .Where(a => a.GiftUserType == GiftUserTypeEnum.TimePrice)
        //        .GroupBy(a => a.CurrentPrice)
        //        .Select(a => new
        //        {
        //            Count = a.Count(),
        //            CurrentPrice = a.Key
        //        })
        //        .Select(a => $"{a.Count}个{a.CurrentPrice}")
        //        .ToList();
        //    var giftStr = $"\r\n其中 {string.Join("    ", giftList)}";

        //    //冠亚免
        //    var freeList = result.SequenceRecordItems
        //        .Where(a => a.GiftUserType == GiftUserTypeEnum.Free)
        //        .ToList();
        //    var freeStr =
        //        freeList.Any() ?
        //            $"\r\n{freeList.Count}个冠亚免（{string.Join("、", freeList.Select(a => a.ShowName))}）" : "";

        //    //vip免
        //    var vipFreeList = result.SequenceRecordItems
        //        .Where(a => a.GiftUserType == GiftUserTypeEnum.VipUser)
        //        .ToList();
        //    var vipFreeStr =
        //        vipFreeList.Any() ?
        //            $"\r\n{vipFreeList.Count}个Vip免（{string.Join("、", vipFreeList.Select(a => a.ShowName))}）" : "";

        //    var payUserCount = result.SequenceRecordItems
        //        .Count(a => a.GiftUserType == GiftUserTypeEnum.Normal ||
        //                    a.GiftUserType == GiftUserTypeEnum.TimePrice);

        //    result.ShowText = $"总：{result.SequenceRecordItems.Count} {payUserCount}付款 {cardStr}" +
        //                      $"{giftStr}" +
        //                      $"{freeStr}" +
        //                      $"{vipFreeStr}";

        //}
    }
}
