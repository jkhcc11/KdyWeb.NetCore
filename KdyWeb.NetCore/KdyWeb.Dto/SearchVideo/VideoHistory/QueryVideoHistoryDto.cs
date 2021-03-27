using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{

    /// <summary>
    /// 查询视频播放记录 Dto
    /// </summary>
    [AutoMap(typeof(VideoHistory))]
    public class QueryVideoHistoryDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 主表主键Key
        /// </summary>
        public long KeyId { get; set; }

        /// <summary>
        /// 剧集Key
        /// </summary>
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
        /// 海报
        /// </summary>
        public string VodImgUrl { get; set; }
    }
}
