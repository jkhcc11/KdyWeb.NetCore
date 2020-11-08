using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 影片主表 Map
    /// </summary>
    public class VideoMainMap : KdyBaseMap<VideoMain, long>
    {
        public VideoMainMap() : base("VideoMain")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoMain> builder)
        {
            //主->扩展
            builder.HasOne(a => a.VideoMainInfo)
                .WithOne(a => a.VideoMain)
                .HasForeignKey<VideoMainInfo>(a => a.MainId);

            //主->组
            builder.HasMany(a => a.EpisodeGroup)
                .WithOne(a => a.VideoMain)
                .HasForeignKey(a => a.MainId);
        }
    }
}
