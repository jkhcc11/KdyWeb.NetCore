using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// Old影视
    /// </summary>
    public class OldSearchSysMainMap : KdyBaseMap<OldSearchSysMain, int>
    {
        public OldSearchSysMainMap() : base("Old.SearchSys.Main")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldSearchSysMain> builder)
        {
            builder.Property(a => a.Id).HasColumnName("KeyId");
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.IsDelete)
                .Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);

            //主->剧集
            builder.HasMany(a => a.Episodes)
                .WithOne(a => a.Main)
                .HasForeignKey(a => a.KeyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
