﻿using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// Cookie类型 服务实现
    /// </summary>
    public class CloudParseCookieTypeService : BaseKdyService, ICloudParseCookieTypeService
    {
        private readonly IKdyRepository<CloudParseCookieType, long> _cloudParseCookieTypeRepository;
        private readonly ISubAccountService _subAccountService;

        public CloudParseCookieTypeService(IUnitOfWork unitOfWork,
            IKdyRepository<CloudParseCookieType, long> cloudParseCookieTypeRepository, 
            ISubAccountService subAccountService) : base(unitOfWork)
        {
            _cloudParseCookieTypeRepository = cloudParseCookieTypeRepository;
            _subAccountService = subAccountService;
        }

        /// <summary>
        /// 查询Cookie类型列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryCookieTypeDto>>> QueryCookieTypeAsync(QueryCookieTypeInput input)
        {
            var query = _cloudParseCookieTypeRepository.GetQuery();
            var result = await query.GetDtoPageListAsync<CloudParseCookieType, QueryCookieTypeDto>(input);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建和更新Cookie类型
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateCookieTypeAsync(CreateAndUpdateCookieTypeInput input)
        {
            var cookieTypeQuery = _cloudParseCookieTypeRepository.GetQuery();
            cookieTypeQuery = cookieTypeQuery.Where(a => a.BusinessFlag == input.BusinessFlag);

            if (input.Id.HasValue)
            {
                cookieTypeQuery = cookieTypeQuery.Where(a => a.Id != input.Id);
                if (await cookieTypeQuery.AnyAsync())
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败,业务标识已存在");
                }

                //修改
                var dbEntity = await _cloudParseCookieTypeRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
                if (dbEntity == null)
                {
                    return KdyResult.Error(KdyResultCode.Error, "修改失败,暂无信息");
                }

                dbEntity.SetShowText(input.ShowText);
                dbEntity.SetBusinessFlag(input.BusinessFlag);
                //dbEntity.ShowText = input.ShowText;
                //dbEntity.BusinessFlag = input.BusinessFlag;
                _cloudParseCookieTypeRepository.Update(dbEntity);
            }
            else
            {
                if (await cookieTypeQuery.AnyAsync())
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败,业务标识已存在");
                }

                //新增
                var dbEntity = new CloudParseCookieType(input.ShowText, input.BusinessFlag);
                await _cloudParseCookieTypeRepository.CreateAsync(dbEntity);
            }

            await _subAccountService.ClearAllCookieTypeCacheAsync();
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbEntities = await _cloudParseCookieTypeRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _cloudParseCookieTypeRepository.Delete(dbEntities);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("删除成功");
        }
    }
}