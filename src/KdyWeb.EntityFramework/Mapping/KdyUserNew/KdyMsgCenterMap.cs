using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.EntityFramework.Mapping.KdyUserNew
{
    /// <summary>
    /// 消息中心 Map
    /// </summary>
    public class KdyMsgCenterMap : KdyBaseMap<KdyMsgCenter, long>
    {
        public KdyMsgCenterMap() : base("KdyBase_KdyMsgCenter")
        {

        }
    }
}
