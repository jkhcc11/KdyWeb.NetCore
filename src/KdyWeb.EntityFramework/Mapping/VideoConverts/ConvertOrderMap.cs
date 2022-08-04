using KdyWeb.Entity.VideoConverts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping.VideoConverts
{
    /// <summary>
    /// 转换订单Map
    /// </summary>
    public class ConvertOrderMap : KdyBaseMap<ConvertOrder, long>
    {
        public ConvertOrderMap() : base("KdyTask_ConvertOrder")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<ConvertOrder> builder)
        {
            //主->详情
            builder.HasMany(a => a.OrderDetails)
                .WithOne()
                .HasForeignKey(a => a.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
