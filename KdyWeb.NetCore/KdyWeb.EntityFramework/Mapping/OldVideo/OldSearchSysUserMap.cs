using System;
using System.Collections.Generic;
using System.Text;
using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldSearchSysUserMap : KdyBaseMap<OldSearchSysUser, int>
    {
        public OldSearchSysUserMap() : base("Old.SearchSys.User")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldSearchSysUser> builder)
        {
            // builder.Property(a => a.Id).HasColumnName("KeyId");
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.IsDelete)
                .Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
