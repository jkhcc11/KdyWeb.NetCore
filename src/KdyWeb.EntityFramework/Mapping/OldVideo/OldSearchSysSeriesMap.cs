using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldSearchSysSeriesMap : KdyBaseMap<OldSearchSysSeries, int>
    {
        public OldSearchSysSeriesMap() : base("Old.SearchSys.Series")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldSearchSysSeries> builder)
        {
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
