using KdyWeb.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 菜单 Map
    /// </summary>
    public class KdyMenuMap : KdyBaseMap<KdyMenu, int>
    {
        public override void MapperConfigure(EntityTypeBuilder<KdyMenu> builder)
        {
            builder.HasMany(a => a.KdyRoleMenus)
                .WithOne(a => a.KdyMenu)
                .HasForeignKey(a => a.MenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
