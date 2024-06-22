using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Resource;
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
                    .Remark?.Replace("\n", "<br/>"),
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
            if (result.LinkItems.Any() == false)
            {
                result.LinkItems = BuildDefaultLink();
            }

            result.QueryFilterItems.AddRange(subTypeFilter);
            result.QueryFilterItems.AddRange(genreFilter);
            result.QueryFilterItems.AddRange(countriesFilter);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建|更新资源
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateResourceAsync(CreateAndUpdateResourceInput input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateResourceAsync(input);
            }

            return await CreateResourceAsync(input);
        }

        /// <summary>
        /// 查询资源列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryResourceDto>>> QueryResourceAsync(QueryResourceInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>
            {
                new()
                {
                    Key = nameof(SysBaseConfig.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var pageList = await _sysBaseConfigRepository.GetQuery()
                .GetDtoPageListAsync<SysBaseConfig, QueryResourceDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 启用资源
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> OpenResourceAsync(long configId)
        {
            var dbEntity = await _sysBaseConfigRepository.FirstOrDefaultAsync(a => a.Id == configId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            dbEntity.Open();
            _sysBaseConfigRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 禁用资源
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BanResourceAsync(long configId)
        {
            var dbEntity = await _sysBaseConfigRepository.FirstOrDefaultAsync(a => a.Id == configId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            dbEntity.Ban();
            _sysBaseConfigRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        private List<NavItem> BuildDefaultNav()
        {
            return new List<NavItem>
            {
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
                //new()
                //{
                //    ShowName = "他们正在看",
                //    Value = "/real-rank",
                //},
                //new()
                //{
                //    ShowName = "万能搜索",
                //    Value = "/bt-search",
                //}
            };
        }

        private List<LinkItem> BuildDefaultLink()
        {
            return new List<LinkItem>
            {
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

            } ;
        }

        private async Task<KdyResult> CreateResourceAsync(CreateAndUpdateResourceInput input)
        {
            if (await _sysBaseConfigRepository.GetAsNoTracking()
                    .AnyAsync(a => a.ConfigName == input.ConfigName &&
                                   a.ConfigType == input.ConfigType))
            {
                return KdyResult.Error(KdyResultCode.Error, "同类型同名称已存在");
            }

            var dbEntity = new SysBaseConfig(input.ConfigType, input.ConfigName, input.TargetUrl)
            {
                ImgUrl = input.ImgUrl,
                Remark = input.Remark
            };

            await _sysBaseConfigRepository.CreateAsync(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        private async Task<KdyResult> UpdateResourceAsync(CreateAndUpdateResourceInput input)
        {
            var dbEntity = await _sysBaseConfigRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            if (await _sysBaseConfigRepository.GetAsNoTracking()
                    .AnyAsync(a => a.ConfigName == input.ConfigName &&
                                   a.ConfigType == input.ConfigType &&
                                   a.Id != input.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "同类型同名称已存在");
            }

            dbEntity.ConfigName = input.ConfigName;
            dbEntity.TargetUrl = input.TargetUrl;
            dbEntity.Remark = input.Remark;
            dbEntity.ImgUrl = input.ImgUrl;
            dbEntity.SetType(input.ConfigType);

            _sysBaseConfigRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
