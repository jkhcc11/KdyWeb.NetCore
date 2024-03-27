using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 创建和更新角色菜单
    /// </summary>
    public class CreateAndUpdateRoleMenuInput
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 菜单Item
        /// </summary>
        public List<CreateAndUpdateRoleMenuItem> MenuItems { get; set; }
    }

    public class CreateAndUpdateRoleMenuItem
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
    }
}
