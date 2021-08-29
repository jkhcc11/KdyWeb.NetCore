using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 用户播放记录分页查询 Dto
    /// </summary>
    [AutoMap(typeof(UserHistory))]
    public class QueryUserHistoryDto : CreatedUserDto<long>
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
        /// 播放地址
        /// </summary>
        public string VodUrl { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
