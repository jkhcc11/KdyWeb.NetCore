using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SequenceRecord;
using KdyWeb.Entity.SequenceRecord;
using KdyWeb.IService.SequenceRecord;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SequenceRecord
{
    /// <summary>
    /// 场馆配置 服务实现
    /// </summary>
    public class VenuesConfigService : BaseKdyService, IVenuesConfigService
    {
        private readonly IKdyRepository<VenuesConfig, long> _venuesConfigRepository;

        public VenuesConfigService(IUnitOfWork unitOfWork, IKdyRepository<VenuesConfig, long> venuesConfigRepository) : base(unitOfWork)
        {
            _venuesConfigRepository = venuesConfigRepository;
        }

        /// <summary>
        /// 分页查询场馆配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryPageVenuesConfigDto>>> QueryPageVenuesConfigAsync(QueryPageVenuesConfigInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new()
                {
                    Key = nameof(VenuesConfig.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _venuesConfigRepository.GetQuery();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.VenuesName.Contains(input.KeyWord) ||
                                         a.ShortName.Contains(input.KeyWord));
            }

            var pageList = await query
                .GetDtoPageListAsync<VenuesConfig, QueryPageVenuesConfigDto>(input);
            return KdyResult.Success(pageList, "操作成功");

        }

        /// <summary>
        /// 创建场馆配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateVenuesConfigAsync(CreateVenuesConfigInput input)
        {
            if (await _venuesConfigRepository.GetQuery().AnyAsync(a => a.VenuesName == input.VenuesName))
            {
                return KdyResult.Error(KdyResultCode.Error, "场馆已存在");
            }

            var entity = new VenuesConfig(input.VenuesName, input.ShortName)
            {
                MaxPrice = input.MaxPrice,
                MinPrice = input.MinPrice,
                NumberSteps = input.NumberSteps,
                VipFreeSteps = input.VipFreeSteps
            };
            await _venuesConfigRepository.CreateAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 禁用场馆奖励计算
        /// </summary>
        /// <param name="venuesConfigId">场馆配置Id</param>
        /// <returns></returns>
        public async Task<KdyResult> BanVenuesGiftAsync(long venuesConfigId)
        {
            var entity = await _venuesConfigRepository.GetQuery()
                .FirstOrDefaultAsync(a => a.Id == venuesConfigId);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "场馆配置不存在");
            }

            if (entity.IsEnableGift)
            {
                entity.Ban();
            }
            else
            {
                entity.Enable();
            }

            _venuesConfigRepository.Update(entity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
