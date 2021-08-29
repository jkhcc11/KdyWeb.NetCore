using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldFeedBackInfoMap : KdyBaseMap<OldFeedBackInfo, int>
    {
        public OldFeedBackInfoMap() : base("Old.SearchSys.Wait")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldFeedBackInfo> builder)
        {
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
