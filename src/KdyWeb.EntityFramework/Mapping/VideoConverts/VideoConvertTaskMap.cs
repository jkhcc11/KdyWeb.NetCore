using KdyWeb.Entity.VideoConverts;

namespace KdyWeb.EntityFramework.Mapping.VideoConverts
{
    /// <summary>
    /// 视频转码任务Map
    /// </summary>
    public class VideoConvertTaskMap : KdyBaseMap<VideoConvertTask, long>
    {
        public VideoConvertTaskMap() : base("KdyTask_VideoConvertTask")
        {

        }
    }
}
