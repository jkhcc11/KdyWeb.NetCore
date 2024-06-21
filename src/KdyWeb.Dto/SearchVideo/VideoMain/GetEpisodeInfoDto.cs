using System.Linq;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 根据剧集Id获取影片数据 Dto
    /// </summary>
    [AutoMap(typeof(VideoEpisode))]
    public class GetEpisodeInfoDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 播放器Host
        /// </summary>
        public string PlayerHost { get; set; } = "//kdy-play.kdy666.pro";

        /// <summary>
        /// 剧集Url
        /// </summary>
        public string EpisodeUrl { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        public string EpisodeName { get; set; }

        /// <summary>
        /// 剧集组
        /// </summary>
        public VideoEpisodeGroupDto VideoEpisodeGroup { get; set; }

        /// <summary>
        /// 影片信息
        /// </summary>
        public VideoMainDto VideoMainInfo { get; set; }

        /// <summary>
        /// 请求清空EpUrl
        /// </summary>
        public void ClearEpUrl()
        {
            EpisodeUrl = string.Empty;
            if (VideoEpisodeGroup != null &&
                VideoEpisodeGroup.Episodes.Any())
            {
                VideoEpisodeGroup.Episodes.ForEach((item) =>
                {
                    item.EpisodeUrl = string.Empty;
                });
            }
        }
    }

    /// <summary>
    /// 影片信息
    /// </summary>
    [AutoMap(typeof(VideoMain))]
    public class VideoMainDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        public string SubtypeVal => Subtype.ToString().ToLower();

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
    }
}
