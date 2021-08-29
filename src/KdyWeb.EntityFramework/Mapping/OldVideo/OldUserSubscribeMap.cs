using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldUserSubscribeMap : KdyBaseMap<OldUserSubscribe, int>
    {
        public OldUserSubscribeMap() : base("Old.SearchSys.Subscribe")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldUserSubscribe> builder)
        {
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
