using KdyWeb.Entity;
using KdyWeb.Entity.KdyUserNew;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.EntityFramework.Mapping.KdyUserNew
{
    /// <summary>
    /// 菜单 Map
    /// </summary>
    public class KdyMenuNewMap : KdyBaseMap<KdyMenuNew, long>
    {
        public KdyMenuNewMap() : base("KdyBase_KdyMenu")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<KdyMenuNew> builder)
        {
            builder.HasMany(a => a.KdyRoleMenuNews)
                .WithOne(a => a.KdyMenuNew)
                .HasForeignKey(a => a.MenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
