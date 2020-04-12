using System;
using KdyWeb.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 用户角色Map
    /// </summary>
    public class KdyRoleMap : KdyBaseMap<KdyRole, int>
    {
        public override void MapperConfigure(EntityTypeBuilder<KdyRole> builder)
        {
            #region 种子数据
            builder.HasData(
                   new KdyRole()
                   {
                       Id = 1,
                       KdyRoleType = KdyRoleType.Normal,
                       CreatedTime = DateTime.Now,
                       IsActivate = true
                   }, new KdyRole()
                   {
                       Id = 2,
                       KdyRoleType = KdyRoleType.VideoAdmin,
                       CreatedTime = DateTime.Now,
                       IsActivate = true
                   },
                   new KdyRole()
                   {
                       Id = 3,
                       KdyRoleType = KdyRoleType.SupperAdmin,
                       CreatedTime = DateTime.Now,
                       IsActivate = true
                   },
                   new KdyRole()
                   {
                       Id = 4,
                       KdyRoleType = KdyRoleType.LiveAdmin,
                       CreatedTime = DateTime.Now,
                       IsActivate = true
                   });
            #endregion

            builder.HasMany(a => a.KdyRoleMenus)
                .WithOne(a => a.KdyRole)
                .HasForeignKey(a => a.RoleId);

            builder.HasMany(a => a.KdyUsers)
                .WithOne(a => a.KdyRole)
                .HasForeignKey(a => a.KdyRoleId);

            builder.Property(a => a.KdyRoleType).HasDefaultValue(KdyRoleType.Normal);
        }
    }
}
