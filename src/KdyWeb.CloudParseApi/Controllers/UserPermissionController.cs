using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.IService.CloudParse;
using Microsoft.AspNetCore.Mvc;
using static KdyWeb.BaseInterface.AuthorizationConst;

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

            if (_loginUserInfo.IsVodAdmin)
            {
                return KdyResult.Success(BuildVodAdmin());
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
                        }
                    }
                },
                BuildDataStatisticsMenu(),
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
                        }
                    }
                },
                BuildDataStatisticsMenu(),
                BuildVodManagerMenu(),
                BuildTaskMenu()
            };

            return result;
        }

        private List<GetVueMenuWithWorkVueDto> BuildVodAdmin()
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
                            MenuUrl = "/index/work-place-vod",
                            MenuName ="工作台",
                            RouteName = "WorkPlaceVod",
                            LocalFilePath = "/index/work-place-vod",
                            IsRootPath = true
                        }
                    }
                },
                BuildVodManagerMenu(),
                BuildTaskMenu(),
            };

            if (_loginUserInfo.UserName == ShareUserName)
            {
                result.Add(BuildCloudParseMenu());
                result.Add(new GetVueMenuWithWorkVueDto()
                {
                    MenuUrl = "/system",
                    MenuName = "系统设置",
                    IconPrefix = "iconfont",
                    Icon = "setting",
                    RouteName = "System",
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
                        }
                    }
                });
            }

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
                        MenuUrl = "/cloud-disk/tx-share",
                        MenuName = "腾讯云分享Beta",
                        RouteName = "TxShareList",
                        LocalFilePath = "/cloudDisk/tx-share",
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
                                RouteName = "TyPersonList",
                                Cacheable = true
                            },
                            new()
                            {
                                ParentPath = "/cloud-disk/ty",
                                MenuUrl = "/cloud-disk/ty/family-list",
                                LocalFilePath = "/cloudDisk/ty/family-list",
                                MenuName = "家庭",
                                RouteName = "TyFamilyList",
                                Cacheable = true
                            },
                            new()
                            {
                                ParentPath = "/cloud-disk/ty",
                                MenuUrl = "/cloud-disk/ty/crop-list",
                                LocalFilePath = "/cloudDisk/ty/crop-list",
                                MenuName = "企业",
                                RouteName = "TyCropList",
                                Cacheable = true
                            },
                        }
                    },
                }
            };
        }

        private GetVueMenuWithWorkVueDto BuildDataStatisticsMenu()
        {
            return new GetVueMenuWithWorkVueDto()
            {
                MenuUrl = "/data-statistics",
                MenuName = "数据统计",
                IconPrefix = "iconfont",
                Icon = "detail",
                RouteName = "DataStatistics",
                Children = new List<GetVueMenuWithWorkVueDto>()
                {
                    new()
                    {
                        ParentPath = "/data-statistics",
                        MenuUrl = "/data-statistics/record-history",
                        RouteName = "RecordHistory",
                        LocalFilePath = "/system/parseRecordHistory/list",
                        MenuName = "访问记录",
                        Cacheable = true,
                    },
                    new()
                    {
                        ParentPath = "/data-statistics",
                        MenuUrl = "/data-statistics/top-list",
                        MenuName = "热门",
                        RouteName = "HistoryTop",
                        LocalFilePath = "/system/parseRecordHistory/top-list",
                        Cacheable = true
                    },
                    new()
                    {
                        ParentPath = "/data-statistics",
                        MenuUrl = "/data-statistics/date-sum-list",
                        MenuName = "天汇总",
                        RouteName = "DateSumList",
                        LocalFilePath = "/system/parseRecordHistory/date-sum-list",
                        Cacheable = true
                    },
                }
            };
        }

        private GetVueMenuWithWorkVueDto BuildVodManagerMenu()
        {
            var parentPath = "/resource-manager";
            return new GetVueMenuWithWorkVueDto()
            {
                MenuUrl = parentPath,
                MenuName = "资源管理",
                IconPrefix = "iconfont",
                Icon = "detail",
                RouteName = "ResourceManager",
                Children = new List<GetVueMenuWithWorkVueDto>()
                {
                    new()
                    {
                        ParentPath = parentPath,
                        MenuUrl = $"{parentPath}/fast-update-vod",
                        RouteName = "FastUpdateVod",
                        LocalFilePath = "/vod/vod-update",
                        MenuName = "影片录入",
                        Cacheable = true,
                    },
                    new()
                    {
                        ParentPath = parentPath,
                        MenuUrl = $"{parentPath}/search-vod",
                        RouteName = "SearchVod",
                        LocalFilePath = "/vod/search-vod",
                        MenuName = "录资源",
                        Cacheable = true,
                    },
                    new()
                    {
                        ParentPath = parentPath,
                        MenuUrl = $"{parentPath}/vod-manager-record",
                        RouteName = "VodManagerRecord",
                        LocalFilePath = "/vod/vod-manager-record",
                        MenuName = "管理记录",
                        Cacheable = true,
                    },
                    new()
                    {
                        ParentPath = parentPath,
                        MenuUrl = $"{parentPath}/feed-back-list",
                        RouteName = "FeedbackInfoManager",
                        LocalFilePath = "/vod/feedback-info-manager",
                        MenuName = "反馈列表",
                        Cacheable = true,
                    },
                }
            };
        }

        private GetVueMenuWithWorkVueDto BuildTaskMenu()
        {
            var parentPath = "/task-center";
            var normalTaskUserMenu = BuildNormalTaskUserMenu();
            if (_loginUserInfo.IsSuperAdmin ||
                _loginUserInfo.IsVodAdmin)
            {
                normalTaskUserMenu.AddRange(new List<GetVueMenuWithWorkVueDto>()
                {
                    new()
                    {
                        ParentPath = parentPath,
                        MenuUrl = $"{parentPath}/task-list-manager",
                        RouteName = "TaskListManager",
                        LocalFilePath = "/taskCenter/task-list-manager",
                        MenuName = "管理任务",
                        Cacheable = true,
                    },
                    new()
                    {
                        ParentPath = parentPath,
                        MenuUrl = $"{parentPath}/convert-order-list",
                        RouteName = "ConvertOrderList",
                        LocalFilePath = "/taskCenter/convert-order-list",
                        MenuName = "管理订单",
                        Cacheable = true,
                    },
                });
            }
            return new GetVueMenuWithWorkVueDto()
            {
                MenuUrl = parentPath,
                MenuName = "任务中心",
                IconPrefix = "iconfont",
                Icon = "detail",
                RouteName = "TaskCenter",
                Children = normalTaskUserMenu
            };
        }

        /// <summary>
        /// 接单用户菜单
        /// </summary>
        /// <returns></returns>
        private List<GetVueMenuWithWorkVueDto> BuildNormalTaskUserMenu()
        {
            var parentPath = "/task-center";
            var normalMenu = new List<GetVueMenuWithWorkVueDto>()
            {
                new()
                {
                    ParentPath = parentPath,
                    MenuUrl = $"{parentPath}/waiting-task-list",
                    RouteName = "WaitingTaskList",
                    LocalFilePath = "/taskCenter/waiting-task-list",
                    MenuName = "待接单",
                    Cacheable = true,
                },
                new()
                {
                    ParentPath = parentPath,
                    MenuUrl = $"{parentPath}/me-task-list",
                    RouteName = "MeTaskList",
                    LocalFilePath = "/taskCenter/me-task-list",
                    MenuName = "我的任务",
                    Cacheable = true,
                },
                new()
                {
                    ParentPath = parentPath,
                    MenuUrl = $"{parentPath}/me-order-list",
                    RouteName = "MeConvertOrderList",
                    LocalFilePath = "/taskCenter/me-convert-order-list",
                    MenuName = "我的订单",
                    Cacheable = true,
                },
            };

            return normalMenu;
        }
    }
}
