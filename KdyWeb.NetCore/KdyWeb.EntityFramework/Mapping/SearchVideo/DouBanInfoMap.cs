using KdyWeb.Entity;
using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 豆瓣信息 Map
    /// </summary>
    public class DouBanInfoMap : KdyBaseMap<DouBanInfo, int>
    {

        public DouBanInfoMap() : base("DouBanInfo")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<DouBanInfo> builder)
        {
            builder.Property(a => a.Subtype).HasDefaultValue(Subtype.None);
            builder.Property(a => a.DouBanInfoStatus).HasDefaultValue(DouBanInfoStatus.SearchWait);
        }
    }
}
