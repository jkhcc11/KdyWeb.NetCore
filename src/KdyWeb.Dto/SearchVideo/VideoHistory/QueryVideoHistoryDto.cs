using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;
using Newtonsoft.Json;

namespace KdyWeb.Dto.SearchVideo
{

    /// <summary>
    /// 查询视频播放记录 Dto
    /// </summary>
    [AutoMap(typeof(VideoHistory))]
    public class QueryVideoHistoryDto : CreatedUserDto<long>, IBaseImgUrl
    {
        /// <summary>
        /// 主表主键Key
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long KeyId { get; set; }

        /// <summary>
        /// 剧集Key
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long EpId { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        public string EpName { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        public string VodName { get; set; }

        /// <summary>
        /// show海报
        /// </summary>
        public string VodImgUrl => VideoImg;

        public string VideoImg { get; set; } = "";
    }
}
