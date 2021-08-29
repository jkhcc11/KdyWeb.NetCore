using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// Old剧集
    /// </summary>
    public class OldSearchSysEpisodeMap : KdyBaseMap<OldSearchSysEpisode, int>
    {
        public OldSearchSysEpisodeMap() : base("Old.SearchSys.Episode")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldSearchSysEpisode> builder)
        {
            builder.Property(a => a.Id).HasColumnName("EpId");
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.IsDelete)
                .Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
