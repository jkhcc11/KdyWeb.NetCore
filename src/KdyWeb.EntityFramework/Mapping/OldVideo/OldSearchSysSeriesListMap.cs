using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldSearchSysSeriesListMap : KdyBaseMap<OldSearchSysSeriesList, int>
    {
        public OldSearchSysSeriesListMap() : base("Old.SearchSys.SeriesList")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldSearchSysSeriesList> builder)
        {
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
