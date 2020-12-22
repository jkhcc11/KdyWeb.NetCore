using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

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
