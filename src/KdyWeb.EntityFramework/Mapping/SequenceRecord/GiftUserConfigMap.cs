using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.EntityFramework.Mapping.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置 Map
    /// </summary>
    public class GiftUserConfigMap : KdyBaseMap<GiftUserConfig, long>
    {
        public GiftUserConfigMap() : base("SequenceRecord_GiftUserConfig")
        {

        }
    }
}
