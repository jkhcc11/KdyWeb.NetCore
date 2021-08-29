using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 影片下载地址 Map
    /// </summary>
    public class VideoDownInfoMap : KdyBaseMap<VideoDownInfo, long>
    {
        public VideoDownInfoMap() : base("VideoDownInfo")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoDownInfo> builder)
        {

        }
    }
}
