using System.Collections.Generic;
using System.Net;
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
    [CustomRoute("user-permission")]
    public class UserPermissionController : BaseApiController
    {
        private readonly ICloudParseUserService _cloudParseUserService;
        private readonly ILoginUserInfo _loginUserInfo;

        public UserPermissionController(ICloudParseUserService cloudParseUserService,
            ILoginUserInfo loginUserInfo)
        {
            _cloudParseUserService = cloudParseUserService;
            _loginUserInfo = loginUserInfo;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-menu")]
        [ProducesResponseType(typeof(KdyResult<List<GetVueMenuWithWorkVueDto>>), (int)HttpStatusCode.OK)]
        public async Task<KdyResult<List<GetVueMenuWithWorkVueDto>>> GetMenuAsync()
        {
            var loginInfo = await _cloudParseUserService.GetParseUserInfoAsync();
            var isFirst = loginInfo.Data.UserStatus == ServerCookieStatus.Init;
            var isRoot = _loginUserInfo.IsSuperAdmin;
            if (isFirst)
            {
                return KdyResult.Success(BuildFirst());
            }

            if (isRoot)
            {
                return KdyResult.Success(BuildAdmin());
            }

            return KdyResult.Success(BuildNormal());
        }

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

        private List<GetVueMenuWithWorkVueDto> BuildNormal()
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
                            MenuUrl = "/index/work-place",
                            MenuName ="工作台",
                            RouteName = "RegIndex",
                            IsRootPath = true
                        }
                    }
                },
                BuildCloudParseMenu(),
                new ()
                {
                    MenuUrl = "/system",
                    MenuName = "系统设置",
                    IconPrefix = "iconfont",
                    Icon = "setting",
                    RouteName="System",
                    Children = new List<GetVueMenuWithWorkVueDto>()
                    {
                        new()
                        {
                            ParentPath = "/system", //得跟上级MenuUrl 一致 当前munuUrl前缀必须跟当前一直 否则刷新后无法选中
                            RouteName="SubAccount",
                            MenuUrl = "/system/sub-account-list",
                            LocalFilePath = "/system/subAccount/subAccount-list",
                            MenuName = "子账号",
                            Cacheable = true
                        },
                        new()
                        {
                            ParentPath = "/system",
                            RouteName="ServerCookie",
                            MenuUrl = "/system/server-cookie-list",
                            LocalFilePath = "/system/serverCookie/server-cookie-list",
                            MenuName = "服务器Cookie",
                            Cacheable = true
                        },
                    }
                },
            };

            return result;
        }

        private List<GetVueMenuWithWorkVueDto> BuildAdmin()
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
                            MenuUrl = "/index/work-place",
                            MenuName = "工作台",
                            RouteName = "RegIndex",
                            IsRootPath = true
                        }
                    }
                },
                BuildCloudParseMenu(),
                new ()
                {
                    MenuUrl = "/system",
                    MenuName = "系统设置",
                    IconPrefix = "iconfont",
                    Icon = "setting",
                    RouteName="System",
                    Children = new List<GetVueMenuWithWorkVueDto>()
                    {
                        new()
                        {
                            ParentPath = "/system",
                            RouteName="CookieType",
                            MenuUrl = "/system/cookie-type-list",
                            LocalFilePath = "/system/cookieType/cookie-type-list",
                            MenuName = "Cookie类型",
                            Cacheable = true
                        },
                        new()
                        {
                            ParentPath = "/system", //得跟上级MenuUrl 一致 当前munuUrl前缀必须跟当前一直 否则刷新后无法选中
                            RouteName="SubAccount",
                            MenuUrl = "/system/sub-account-list",
                            LocalFilePath = "/system/subAccount/subAccount-list",
                            MenuName = "子账号",
                            Cacheable = true
                        },
                        new()
                        {
                            ParentPath = "/system",
                            RouteName="ServerCookie",
                            MenuUrl = "/system/server-cookie-list",
                            LocalFilePath = "/system/serverCookie/server-cookie-list",
                            MenuName = "服务器Cookie",
                            Cacheable = true
                        },
                        new()
                        {
                            ParentPath = "/system",
                            RouteName="ParseUser",
                            MenuUrl = "/system/user-list",
                            LocalFilePath = "/system/user/user-list",
                            MenuName = "用户列表",
                            Cacheable = true
                        },
                    }
                },
            };

            return result;
        }

        private GetVueMenuWithWorkVueDto BuildCloudParseMenu()
        {
            return new GetVueMenuWithWorkVueDto()
            {
                MenuUrl = "/cloud-disk",
                MenuName = "网盘列表",
                IconPrefix = "iconfont",
                Icon = "detail",
                RouteName = "CloudDisk",
                Children = new List<GetVueMenuWithWorkVueDto>()
                {
                    new()
                    {
                        ParentPath = "/cloud-disk",
                        MenuUrl = "/cloud-disk/ali-list",
                        RouteName = "AliList",
                        LocalFilePath = "/cloudDisk/ali/ali-list",
                        MenuName = "阿里",
                        Cacheable = true,
                    },
                    new()
                    {
                        ParentPath = "/cloud-disk",
                        MenuUrl = "/cloud-disk/st-list",
                        MenuName = "胜天",
                        RouteName = "StList",
                        LocalFilePath = "/cloudDisk/st-list",
                        Cacheable = true
                    },
                    new()
                    {
                        ParentPath = "/cloud-disk",
                        MenuUrl = "/cloud-disk/hc-list",
                        MenuName = "139盘",
                        RouteName = "HcList",
                        LocalFilePath = "/cloudDisk/139-list",
                        Cacheable = true
                    },
                    new()
                    {
                        ParentPath = "/cloud-disk",
                        MenuUrl = "/cloud-disk/ty",
                        MenuName = "天翼",
                        Children = new List<GetVueMenuWithWorkVueDto>()
                        {
                            new()
                            {
                                ParentPath = "/cloud-disk/ty",
                                MenuUrl = "/cloud-disk/ty/person-list",
                                LocalFilePath = "/cloudDisk/ty/person-list",
                                MenuName = "个人",
                                Cacheable = true
                            },
                            new()
                            {
                                ParentPath = "/cloud-disk/ty",
                                MenuUrl = "/cloud-disk/ty/family-list",
                                LocalFilePath = "/cloudDisk/ty/family-list",
                                MenuName = "家庭",
                                Cacheable = true
                            },
                            new()
                            {
                                ParentPath = "/cloud-disk/ty",
                                MenuUrl = "/cloud-disk/ty/crop-list",
                                LocalFilePath = "/cloudDisk/ty/crop-list",
                                MenuName = "企业",
                                Cacheable = true
                            },
                        }
                    },
                }
            };
        }
    }
}
