using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 剧集组 Map
    /// </summary>
    public class VideoEpisodeGroupMap : KdyBaseMap<VideoEpisodeGroup, long>
    {
        public VideoEpisodeGroupMap() : base("VideoEpisodeGroup")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoEpisodeGroup> builder)
        {
            //组->剧集
            builder.HasMany(a => a.Episodes)
                .WithOne(a => a.VideoEpisodeGroup)
                .HasForeignKey(a => a.EpisodeGroupId);
        }
    }
}
