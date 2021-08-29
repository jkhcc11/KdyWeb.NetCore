using System;
using System.Collections.Generic;
using System.Text;
using KdyWeb.Entity.OldVideo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    public class OldUserHistoryMap : KdyBaseMap<OldUserHistory, int>
    {
        public OldUserHistoryMap() : base("Old.SearchSys.UserHistory")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<OldUserHistory> builder)
        {
            builder.Property(a => a.CreatedTime).HasColumnName("CreateTime");

            builder.Ignore(a => a.CreatedUserId)
                .Ignore(a => a.ModifyUserId);
        }
    }
}
