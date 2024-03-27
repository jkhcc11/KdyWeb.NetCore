using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 用户权限
    /// </summary>
    [CustomRoute("user-permission-new")]
    public class UserPermissionV2Controller : BaseApiController
    {
        private readonly ICloudParseUserService _cloudParseUserService;
        private readonly IKdyRoleMenuNewService _kdyRoleMenuNewService;
        private readonly ILoginUserInfo _loginUserInfo;

        public UserPermissionV2Controller(IKdyRoleMenuNewService kdyRoleMenuNewService,
            ILoginUserInfo loginUserInfo, ICloudParseUserService cloudParseUserService)
        {
            _kdyRoleMenuNewService = kdyRoleMenuNewService;
            _loginUserInfo = loginUserInfo;
            _cloudParseUserService = cloudParseUserService;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-menu")]
        public async Task<KdyResult<List<GetVueMenuWithWorkVueDto>>> GetMenuAsync()
        {
            var loginInfo = await _cloudParseUserService.GetParseUserInfoAsync();
            var isFirst = loginInfo.Data.UserStatus == ServerCookieStatus.Init;
            if (isFirst)
            {
                return KdyResult.Success(BuildFirst());
            }

            if (string.IsNullOrEmpty(_loginUserInfo.RoleName))
            {
                return KdyResult.Error<List<GetVueMenuWithWorkVueDto>>(KdyResultCode.Error, "无效角色名");
            }

            var tempRoles = _loginUserInfo.RoleName.Split(',').ToArray();
            var menuTreeData = await _kdyRoleMenuNewService.GetRoleActivateMenuTreeAsync(tempRoles);
            if (menuTreeData.Data != null &&
                menuTreeData.Data.Any())
            {
                return menuTreeData;
            }

            //没有菜单时
            if (_loginUserInfo.IsSuperAdmin)
            {
                return KdyResult.Success(BuildFirstAdmin());
            }

            return KdyResult.Error<List<GetVueMenuWithWorkVueDto>>(KdyResultCode.Error, "获取菜单失败，请联系管理");
        }

        /// <summary>
        /// 未注册第一次访问
        /// </summary>
        /// <returns></returns>
        private List<GetVueMenuWithWorkVueDto> BuildFirst()
        {
            var result = new List<GetVueMenuWithWorkVueDto>()
            {
                new()
                {
                    MenuUrl = "/index",
                    MenuName = "Dashboard",
                    RouteName = "dashboard",
                    Icon = "icon-dashboard",
                    Children=new List<GetVueMenuWithWorkVueDto>()
                    {
                        new()
                        {
                            ParentPath = "/index",
                            MenuUrl = "/index/reg-index",
                            //LocalFilePath = "/index/reg-index",
                            MenuName = "提交注册",
                            RouteName = "RegIndex",
                            IsRootPath = true
                        }
                    }
                },
            };

            return result;
        }

        /// <summary>
        /// 超管第一次访问
        /// </summary>
        /// <returns></returns>
        private List<GetVueMenuWithWorkVueDto> BuildFirstAdmin()
        {
            var result = new List<GetVueMenuWithWorkVueDto>()
            {
                new()
                {
                    MenuUrl = "/system",
                    MenuName = "Dashboard",
                    RouteName = "dashboard",
                    Icon = "icon-dashboard",
                    Children=new List<GetVueMenuWithWorkVueDto>()
                    {
                        new()
                        {
                            ParentPath = "/system",
                            RouteName="MenuIndex",
                            MenuUrl = "/system/menu-list",
                            LocalFilePath = "/system/role-menu/menu-index",
                            MenuName = "权限菜单",
                            Cacheable = true,
                            IsRootPath = true
                        },
                        new()
                        {
                            ParentPath = "/system",
                            RouteName="RoleIndex",
                            MenuUrl = "/system/role-list",
                            LocalFilePath = "/system/role-menu/role-list",
                            MenuName = "角色列表",
                            Cacheable = true
                        },
                    }
                },
            };

            return result;
        }
    }
}
