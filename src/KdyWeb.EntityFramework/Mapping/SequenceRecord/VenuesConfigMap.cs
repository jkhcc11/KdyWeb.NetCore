using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.EntityFramework.Mapping.SequenceRecord
{
    /// <summary>
    /// 场馆配置 Map
    /// </summary>
    public class VenuesConfigMap : KdyBaseMap<VenuesConfig, long>
    {
        public VenuesConfigMap() : base("SequenceRecord_VenuesConfig")
        {

        }
    }
}
