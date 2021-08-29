using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.IService.HttpCapture;
using KdyWeb.PageParse;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 站点页面搜索配置 服务接口
    /// </summary>
    public class PageSearchConfigService : BaseKdyService, IPageSearchConfigService
    {
        private readonly IKdyRepository<PageSearchConfig, long> _pageSearchConfigRepository;

        public PageSearchConfigService(IUnitOfWork unitOfWork, IKdyRepository<PageSearchConfig, long> pageSearchConfigRepository) : base(unitOfWork)
        {
            _pageSearchConfigRepository = pageSearchConfigRepository;
        }

        /// <summary>
        /// 创建搜索配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateSearchConfigAsync(CreateSearchConfigInput input)
        {
            var check = CheckInput(input);
            if (check.IsSuccess == false)
            {
                return KdyResult.Error(check.Code, check.Msg);
            }

            var exit = await _pageSearchConfigRepository.GetAsNoTracking()
                .AnyAsync(a => a.BaseHost == input.BaseHost);
            if (exit)
            {
                return KdyResult.Error(KdyResultCode.Error, $"站点：{input.BaseHost} 已存在 无需添加");
            }

            var dbInput = input.MapToExt<PageSearchConfig>();
            await _pageSearchConfigRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改搜索配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifySearchConfigAsync(ModifySearchConfigInput input)
        {
            var check = CheckInput(input);
            if (check.IsSuccess == false)
            {
                return KdyResult.Error(check.Code, check.Msg);
            }

            var dbConfig = await _pageSearchConfigRepository.FirstOrDefaultAsync(a => a.Id == input.ConfigId);
            if (dbConfig == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "修改失败，配置Id无效");
            }

            ModifySearchConfigInput.ToDbConfig(dbConfig, input);
            _pageSearchConfigRepository.Update(dbConfig);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取配置详情
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetDetailConfigDto>> GetDetailConfigAsync(long configId)
        {
            var dbConfig = await _pageSearchConfigRepository.FirstOrDefaultAsync(a => a.Id == configId);
            if (dbConfig == null)
            {
                return KdyResult.Error<GetDetailConfigDto>(KdyResultCode.Error, "获取配置详情失败，Id错误");
            }

            //if (dbConfig.SearchConfigStatus != SearchConfigStatus.Normal)
            //{
            //    return KdyResult.Error<GetDetailConfigDto>(KdyResultCode.Error, $"配置Id:{configId},配置非正常状态");
            //}
            var result = dbConfig.MapToExt<GetDetailConfigDto>();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 搜索配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<GetDetailConfigDto>>> SearchConfigAsync(SearchConfigInput input)
        {
            var result = await _pageSearchConfigRepository.GetAsNoTracking()
                .GetDtoPageListAsync<PageSearchConfig, GetDetailConfigDto>(input);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 获取页面搜索实例
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetPageParseInstanceDto>> GetPageParseInstanceAsync(GetPageParseInstanceInput input)
        {
            if (input.ConfigId.HasValue == false &&
                input.ConfigId <= 0 &&
                string.IsNullOrEmpty(input.BaseHost))
            {
                return KdyResult.Error<GetPageParseInstanceDto>(KdyResultCode.Error, "域名或配置Id不能为空");
            }

            var query = _pageSearchConfigRepository.GetAsNoTracking();
            if (input.ConfigId != null)
            {
                query = query.Where(a => a.Id == input.ConfigId);
            }
            else
            {
                query = query.Where(a => a.BaseHost.EndsWith(input.BaseHost) ||
                                         a.OtherHost.Contains(input.BaseHost));
            }

            //配置类名
            var dbTemp = await query.Select(a => new
            {
                a.Id,
                a.ServiceFullName
            }).FirstOrDefaultAsync();
            if (dbTemp == null)
            {
                return KdyResult.Error<GetPageParseInstanceDto>(KdyResultCode.Error, $"ConfigId:{input.ConfigId},BaseHost:{input.BaseHost} 当前搜索配置未生效");
            }

            var pageParseService = KdyBaseServiceProvider.HttpContextServiceProvide.GetServices<IPageParseService<NormalPageParseOut, NormalPageParseInput>>();
            var service = pageParseService.First(a => a.GetType().FullName == dbTemp.ServiceFullName);

            var result = new GetPageParseInstanceDto()
            {
                Instance = service,
                ConfigId = dbTemp.Id
            };

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 一键复制站点配置
        /// </summary>
        /// <param name="oldKeyId">旧Id</param>
        /// <returns></returns>
        public async Task<KdyResult> OneCopyAsync(long oldKeyId)
        {
            if (oldKeyId <= 0)
            {
                return KdyResult.Error(KdyResultCode.ParError, "配置Id不能为空");
            }

            var dbOldConfig = await _pageSearchConfigRepository.FirstOrDefaultAsync(a => a.Id == oldKeyId);
            if (dbOldConfig == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "配置不存在");
            }

            var newConfig = dbOldConfig.MapToExt<PageSearchConfig>();
            newConfig.Id = 0;
            newConfig.ModifyTime = null;
            newConfig.ModifyUserId = null;
            newConfig.CreatedUserId = null;
            newConfig.CreatedTime = DateTime.Now;
            newConfig.HostName = $"{newConfig.HostName} 副本";
            await _pageSearchConfigRepository.CreateAsync(newConfig);

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 检查入参
        /// </summary>
        /// <returns></returns>
        private KdyResult CheckInput(BaseSearchConfigInput input)
        {
            if (input.BaseHost.StartsWith("http://") == false &&
                input.BaseHost.StartsWith("https://") == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "搜索站点非https、http开头");
            }

            if (string.IsNullOrEmpty(input.OtherHost))
            {
                input.OtherHost = new Uri(input.BaseHost).Host;
            }

            return KdyResult.Success();
        }
    }
}
