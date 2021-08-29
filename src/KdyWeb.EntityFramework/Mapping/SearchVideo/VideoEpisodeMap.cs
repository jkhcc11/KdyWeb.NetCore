using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 剧集表 Map
    /// </summary>
    public class VideoEpisodeMap : KdyBaseMap<VideoEpisode, long>
    {
        public VideoEpisodeMap() : base("VideoEpisode")
        {

        }
    }
}
