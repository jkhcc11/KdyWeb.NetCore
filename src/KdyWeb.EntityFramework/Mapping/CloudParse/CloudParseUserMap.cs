using System;
using KdyWeb.Entity.CloudParse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 云盘用户 Map
    /// </summary>
    public class CloudParseUserMap : KdyBaseMap<CloudParseUser, long>
    {
        public CloudParseUserMap() : base("CloudParse_User")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<CloudParseUser> builder)
        {
            builder.Property(a => a.HoldLinkHost)
                .HasConversion(
                    a => string.Join(',', a),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            builder.HasMany(a => a.CloudParseUserChildrens)
                .WithOne(a => a.CloudParseUser)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
