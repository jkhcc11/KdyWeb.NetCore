using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 影片主表扩展信息 Map
    /// </summary>
    public class VideoMainInfoMap : KdyBaseMap<VideoMainInfo, long>
    {
        public VideoMainInfoMap() : base("VideoMainInfo")
        {

        }
    }
}
