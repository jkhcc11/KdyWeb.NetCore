using KdyWeb.BaseInterface.KdyOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using KdyWeb.BaseInterface.Extensions;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取腾讯文档接龙列表 out
    /// </summary>
    public class GetSequenceRecordsOut
    {
        /// <summary>
        /// 创建人昵称
        /// </summary>
        public string CreateUserNick { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 文档标题
        /// </summary>
        /// <remarks>
        /// 周三鼎昇0528
        /// 周五鼎昇双打磨合局0523
        /// </remarks>
        public string DocTitle { get; set; }

        /// <summary>
        /// 活动时间  yyyyMMdd 格式化
        /// </summary>
        public DateTime ActivityTime => DateTime.ParseExact($"{DateTime.Now.Year}" + DocTitle.Substring(DocTitle.Length - 4, 4), "yyyyMMdd", null);

        /// <summary>
        /// 地点
        /// </summary>
        public string PlaceTxt => DocTitle.Substring(2, 2);

        /// <summary>
        /// 接龙记录列表
        /// </summary>
        public List<SequenceRecordItem> SequenceRecordItems { get; set; } = new();

        /// <summary>
        /// 显示文案
        /// </summary>
        public string ShowText { get; set; }

        /// <summary>
        /// 获取缓存Key
        /// </summary>
        /// <returns></returns>
        public string GetCacheKey()
        {
            return $"{ActivityTime:yyyyMMdd}:{PlaceTxt}";
        }

        ///// <summary>
        ///// 统计列表
        ///// </summary>
        //public List<UserTypeItem> TotalItems => SequenceRecordItems
        //    .GroupBy(a => new
        //    {
        //        a.GiftUserType,
        //        a.CurrentPrice
        //    })
        //    .Select(groupItem => new UserTypeItem()
        //    {
        //        CurrentPrice = groupItem.Key.CurrentPrice,
        //        Count = groupItem.Count(),
        //        UserType = groupItem.Key.GiftUserType
        //    })
        //    .ToList();
    }

    ///// <summary>
    ///// 用户类型Item
    ///// </summary>
    //public class UserTypeItem
    //{
    //    /// <summary>
    //    /// 用户类型
    //    /// </summary>
    //    public GiftUserTypeEnum UserType { get; set; }

    //    public string UserTypeStr => UserType.GetDescription();

    //    /// <summary>
    //    /// 当前价格
    //    /// </summary>
    //    public decimal CurrentPrice { get; set; }

    //    /// <summary>
    //    /// 数量
    //    /// </summary>
    //    public int Count { get; set; }
    //}

    /// <summary>
    /// 接龙记录Item
    /// </summary>
    public class SequenceRecordItem
    {
        /// <summary>
        /// 用户唯一Id
        /// </summary>
        public string UId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 接龙名
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 接龙时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
