using System;
using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture.PageParse.CmsJson
{
    /// <summary>
    ///  Cms搜索结果
    /// </summary>
    /// <remarks>
    /// 适用于Json接口
    /// </remarks>
    public class CmsNormalWithSearchResultOut
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("vod_id")]
        public virtual string VodId { get; set; }

        /// <summary>
        /// 搜索结果影片名
        /// </summary>
        [JsonProperty("vod_name")]
        public virtual string VodName { get; set; }

        /// <summary>
        /// 影片类型Id
        /// </summary>
        [JsonProperty("type_id")]
        public virtual int TypeId { get; set; }

        /// <summary>
        /// 影片类型名
        /// </summary>
        [JsonProperty("type_name")]
        public virtual string TypeName { get; set; }

        /// <summary>
        /// 影片更新时间
        /// </summary>
        [JsonProperty("vod_time")]
        public virtual DateTime UpdateDateTime { get; set; }

        /// <summary>
        /// 影片备注
        /// </summary>
        /// <remarks>
        /// 一般都是影片状态 更新至 或者完结
        /// </remarks>
        [JsonProperty("vod_remarks")]
        public virtual string VodRemark { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        public virtual bool IsEnd
        {
            get
            {
                if (string.IsNullOrEmpty(VodRemark))
                {
                    return false;
                }

                return VodRemark.Contains("完") || VodRemark.Contains("全");
            }
        }

        /// <summary>
        /// 播放Url类型
        /// </summary>
        [JsonProperty("vod_play_from")]
        public virtual string VodPlayFrom { get; set; }

        /// <summary>
        /// 播放Url类型列表
        /// </summary>
        public string[] VodPlayFromArray
        {
            get
            {
                return string.IsNullOrEmpty(VodPlayFrom) ? new string[] { } : VodPlayFrom.Split(',');
            }
        }
    }
}
