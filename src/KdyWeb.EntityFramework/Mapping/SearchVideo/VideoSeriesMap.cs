using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    ///  影片系列 Map
    /// </summary>
    public class VideoSeriesMap : KdyBaseMap<VideoSeries, long>
    {
        public VideoSeriesMap() : base("VideoSeries")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoSeries> builder)
        {
            //系列->系列列表
            builder.HasMany(a => a.SeriesList)
                .WithOne(a => a.VideoSeries)
                .HasForeignKey(a => a.SeriesId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
