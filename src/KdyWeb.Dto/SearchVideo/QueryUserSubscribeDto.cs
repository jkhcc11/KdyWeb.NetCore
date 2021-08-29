using System.Collections.Generic;
using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;
using Newtonsoft.Json;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 用户收藏查询 Dto
    /// </summary>
    [AutoMap(typeof(UserSubscribe))]
    public class QueryUserSubscribeDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        /// 影片Id、小说Id
        /// </remarks>
        [JsonConverter(typeof(JsonConverterLong))]
        public long BusinessId { get; set; }

        /// <summary>
        /// 业务特征码
        /// </summary>
        public string BusinessFeature { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户订阅类型
        /// </summary>
        public UserSubscribeType UserSubscribeType { get; set; }

        /// <summary>
        /// 业务信息
        /// </summary>
        public UserSubscribeBusinessItem BusinessItems { get; set; }
    }

    /// <summary>
    /// 业务信息Item
    /// </summary>
    [AutoMap(typeof(VideoMain))]
    public class UserSubscribeBusinessItem : BaseEntityDto<long>
    {

        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 影片类型Str
        /// </summary>
        public string SubtypeStr => Subtype.GetDisplayName();

        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 影片状态
        /// </summary>
        public VideoMainStatus VideoMainStatus { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        public string Aka { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 影片主表 扩展信息
        /// </summary>
        public VideoMainInfoDto VideoMainInfo { get; set; }
    }
}
