using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.Dto.CloudParse.KdyUserNew
{
    /// <summary>
    /// 查询角色列表
    /// </summary>
    public class QueryPageRoleInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(KdyRoleNew.RoleName), KdyOperator.Like)]
        [KdyQuery(nameof(KdyRoleNew.RoleFlag), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
