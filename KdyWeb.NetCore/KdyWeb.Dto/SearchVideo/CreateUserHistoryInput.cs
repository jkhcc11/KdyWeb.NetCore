using AutoMapper;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建用户播放记录 Input
    /// </summary>
    [AutoMap(typeof(UserHistory), ReverseMap = true)]
    public class CreateUserHistoryInput
    {
        /// <summary>
        /// 剧集Id
        /// </summary>
        public long EpId { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string VodUrl { get; set; }
    }
}
