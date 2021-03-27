using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 视频播放记录 Map
    /// </summary>
    public class VideoHistoryMap : KdyBaseMap<VideoHistory, long>
    {
        public VideoHistoryMap() : base("VideoHistory")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoHistory> builder)
        {
            //builder.HasMany(a=>a.VideoMain)
            //    .WithOne()
            //    .HasForeignKey(a=>a)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.VideoMain)
                .WithMany()
                .HasForeignKey(a => a.KeyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
