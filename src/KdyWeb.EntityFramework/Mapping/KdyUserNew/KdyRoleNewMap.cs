using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.EntityFramework.Mapping.KdyUserNew
{
    /// <summary>
    /// 角色 Map
    /// </summary>
    public class KdyRoleNewMap : KdyBaseMap<KdyRoleNew, long>
    {
        public KdyRoleNewMap() : base("KdyBase_KdyRole")
        {

        }
    }
}
