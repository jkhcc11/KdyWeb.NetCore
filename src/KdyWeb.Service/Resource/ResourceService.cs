using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.BaseConfig;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.Resource;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.Resource
{
    /// <summary>
    /// 资源 服务实现
    /// </summary>
    public class ResourceService : BaseKdyService, IResourceService
    {
        private readonly IKdyRepository<SysBaseConfig, long> _sysBaseConfigRepository;

        public ResourceService(IUnitOfWork unitOfWork,
            IKdyRepository<SysBaseConfig, long> sysBaseConfigRepository)
            : base(unitOfWork)
        {
            _sysBaseConfigRepository = sysBaseConfigRepository;
        }

        /// <summary>
        /// 获取全局资源
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetAllResourceDto>> GetAllResourceAsync()
        {
            var dbList = await _sysBaseConfigRepository.GetAsNoTracking()
                .Where(a => a.ConfigStatus == CommonStatusEnum.Normal)
                .ToListAsync();

            #region 过滤生成
            //分类
            var subTypeList = EnumExt.GetEnumList(new[]
            {
                Subtype.Movie, Subtype.Tv, Subtype.Documentary, Subtype.Animation
            });
            var subTypeFilter = subTypeList.Select(a => new QueryFilterItem()
            {
                FilterType = FilterTypeEnum.Type,
                FilterValue = a.EnumValue.ToString().ToLower(),
                ShowName = a.DisplayName
            }).ToList();

            //类型
            var genreFilter = KdyBaseConst.GetAllowGenreArray()
                .Select(a => new QueryFilterItem()
                {
                    FilterType = FilterTypeEnum.Genre,
                    FilterValue = a,
                    ShowName = a
                }).ToList();

            //国家
            var countriesList = EnumExt.GetEnumList(excludeValues: new[]
            {
                VideoCountries.Other
            });
            var countriesFilter = countriesList.Select(a => new QueryFilterItem()
            {
                FilterType = FilterTypeEnum.Country,
                FilterValue = a.EnumValue.ToString().ToLower(),
                ShowName = a.DisplayName
            }).ToList(); 
            #endregion

            var navItems = dbList
                .Where(a => a.ConfigType == ConfigTypeEnum.Nav)
                .Select(a => new NavItem()
                {
                    Value = a.TargetUrl,
                    ShowName = a.ConfigName,
                }).ToList();
            if (navItems.Any() == false)
            {
                navItems = BuildDefaultNav();
            }

            if (LoginUserInfo.IsLogin == false)
            {
                navItems.Add(new NavItem()
                {
                    ShowName = "求片列表",
                    Value = "/feedback"
                });
            }

            var result = new GetAllResourceDto()
            {
                TipMsg = dbList.FirstOrDefault(a => a.ConfigType == ConfigTypeEnum.TipMsg)?
                    .Remark?.Replace("\n","<br/>"),
                LinkItems = dbList
                    .Where(a => a.ConfigType == ConfigTypeEnum.LinkUrl)
                    .Select(a => new LinkItem()
                    {
                        LinkUrl = a.TargetUrl,
                        LinkName = a.ConfigName
                    }).ToList(),
                BannerItems = dbList
                    .Where(a => a.ConfigType == ConfigTypeEnum.Banner)
                    .Select(a => new BannerItem()
                    {
                        Url = a.TargetUrl,
                        BannerName = a.ConfigName,
                        ImgUrl = a.ImgUrl,
                    }).ToList(),
                NavItems = navItems,
                QueryFilterItems = new List<QueryFilterItem>()
            };
            if (result.LinkItems.Any()==false)
            {
                result.LinkItems = BuildDefaultLink();
            }

            result.QueryFilterItems.AddRange(subTypeFilter);
            result.QueryFilterItems.AddRange(genreFilter);
            result.QueryFilterItems.AddRange(countriesFilter);
            return KdyResult.Success(result);
        }

        private List<NavItem> BuildDefaultNav()
        {
            return
            [
                new()
                {
                    ShowName = Subtype.Movie.GetDisplayName(),
                    Value = "/vod/1/movie",
                },
                new()
                {
                    ShowName = Subtype.Tv.GetDisplayName(),
                    Value = "/vod/1/tv",
                },
                new()
                {
                    ShowName = Subtype.Documentary.GetDisplayName(),
                    Value = "/vod/1/documentary",
                },
                new()
                {
                    ShowName = "他们正在看",
                    Value = "/real-rank",
                },
                new()
                {
                    ShowName = "万能搜索",
                    Value = "/bt-search",
                }
            ];
        }

        private List<LinkItem> BuildDefaultLink()
        {
            return
            [
                new()
                {
                    LinkName = "经典影视",
                    LinkUrl = "//plus.17kandy.com",
                },

                new()
                {
                    LinkName = "AI娱乐导航站",
                    LinkUrl = "//www.aifundh.com/",
                },

            ];
        }
    }
}
