using KdyWeb.Entity.VideoConverts;

namespace KdyWeb.EntityFramework.Mapping.VideoConverts
{
    /// <summary>
    /// 转换订单详情Map
    /// </summary>
    public class ConvertOrderDetailMap : KdyBaseMap<ConvertOrderDetail, long>
    {
        public ConvertOrderDetailMap() : base("KdyTask_ConvertOrderDetail")
        {

        }
    }
}
