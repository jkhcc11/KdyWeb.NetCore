using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture.PageParse.CmsJson
{
    /// <summary>
    ///  Cms详情结果
    /// </summary>
    /// <remarks>
    /// 适用于Json接口
    /// </remarks>
    public class CmsNormalWithDetailOut
    {
        /// <summary>
        /// 详情Id
        /// </summary>
        [JsonProperty("vod_id")]
        public virtual int VodId { get; set; }

        /// <summary>
        /// 所属类型Id
        /// </summary>
        [JsonProperty("type_id")]
        public virtual int TypeId { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        [JsonProperty("vod_name")]
        public virtual string VodName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [JsonProperty("vod_pic")]
        public virtual string VodPic { get; set; }

        /// <summary>
        /// 演员
        /// </summary>
        /// <remarks>
        /// 多个用,逗号隔开
        /// </remarks>
        [JsonProperty("vod_actor")]
        public virtual string VodActor { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        /// <remarks>
        /// 多个用,逗号隔开
        /// </remarks>
        [JsonProperty("vod_director")]
        public virtual string VodDirector { get; set; }

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

                return VodRemark.Contains("完") ||
                       VodRemark.Contains("全") ||
                       VodRemark.Contains("HD");
            }
        }

        /// <summary>
        /// 年份
        /// </summary>
        [JsonProperty("vod_year")]
        public virtual int VodYear { get; set; }

        /// <summary>
        /// 豆瓣Id
        /// </summary>
        [JsonProperty("vod_douban_id")]
        public virtual string VodDouBanId { get; set; }

        /// <summary>
        /// 播放Url类型
        /// </summary>
        [JsonProperty("vod_play_from")]
        public virtual string VodPlayFrom { get; set; }

        /// <summary>
        /// 播放Url分割标记
        /// </summary>
        [JsonProperty("vod_play_note")]
        public virtual string VodPlayNote { get; set; }

        /// <summary>
        /// 播放Url
        /// </summary>
        [JsonProperty("vod_play_url")]
        public virtual string VodPlayUrl { get; set; }
    }
}
