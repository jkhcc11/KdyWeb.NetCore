using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldSearchSysDanMuMap : KdyBaseMap<OldSearchSysDanMu, int>
    {
        public OldSearchSysDanMuMap() : base("Old.SearchSys.DanMu")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldSearchSysDanMu> builder)
        {
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
