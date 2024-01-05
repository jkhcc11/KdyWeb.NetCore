using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 影片系列列表 Map
    /// </summary>
    public class VideoSeriesListMap : KdyBaseMap<VideoSeriesList, long>
    {
        public VideoSeriesListMap() : base("VideoSeriesList")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoSeriesList> builder)
        {
            //系列列表->影片主
            builder.HasOne(a => a.VideoMain)
                .WithMany()
                .HasForeignKey(a => a.KeyId)
                .OnDelete(DeleteBehavior.Restrict); ;
        }
    }
}
