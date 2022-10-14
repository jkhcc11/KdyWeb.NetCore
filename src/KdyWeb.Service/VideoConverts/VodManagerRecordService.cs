using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;
using KdyWeb.IRepository.VideoConverts;
using KdyWeb.IService.VideoConverts;
using KdyWeb.Repository;

namespace KdyWeb.Service.VideoConverts
{
    /// <summary>
    /// 影片管理者记录 服务实现
    /// </summary>
    public class VodManagerRecordService : BaseKdyService, IVodManagerRecordService
    {
        private readonly IVodManagerRecordRepository _vodManagerRecordRepository;

        public VodManagerRecordService(IUnitOfWork unitOfWork, IVodManagerRecordRepository vodManagerRecordRepository)
            : base(unitOfWork)
        {
            _vodManagerRecordRepository = vodManagerRecordRepository;
        }

        /// <summary>
        /// 查询影片管理者记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVodManagerRecordDto>>> QueryVodManagerRecordAsync(QueryVodManagerRecordInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(VodManagerRecord.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _vodManagerRecordRepository.GetQuery();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                //非管理员 只能查看自己的操作记录
                query = query.Where(a => a.CreatedUserId == userId);
            }

            var pageList = await query.GetDtoPageListAsync<VodManagerRecord, QueryVodManagerRecordDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 更新记录实际金额
        /// </summary>
        /// <remarks>
        ///  有些积分需要人工核对
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> UpdateRecordActualAmountAsync(UpdateRecordActualAmountInput input)
        {
            var dbEntity = await _vodManagerRecordRepository.FirstOrDefaultAsync(a => a.Id == input.KeyId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "Id无效");
            }

            dbEntity.ActualAmount = input.ActualAmount;
            _vodManagerRecordRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量结算记录
        /// </summary>
        /// <remarks>
        ///  只能全部为有效数据才结算成功
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> BatchCheckoutRecordAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数无效");
            }

            var dbRecord = await _vodManagerRecordRepository.BatchGetRecordByIds(input.Ids);
            if (dbRecord.All(a => a.IsValid) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,存在无效数据");
            }

            dbRecord.ForEach(item =>
            {
                item.IsCheckout = true;
            });
            _vodManagerRecordRepository.Update(dbRecord);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 创建影片管理者记录
        /// </summary>
        /// <remarks>
        /// 对内服务
        /// </remarks>
        /// <param name="recordType">记录类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="businessId">业务Id</param>
        /// <param name="checkoutAmount">结算金额</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public async Task CreateVodManagerRecordAsync(VodManagerRecordType recordType
            , long userId, long businessId, decimal? checkoutAmount = null, string remark = "")
        {
            var entity = new VodManagerRecord(recordType, recordType.GetCheckoutAmount())
            {
                BusinessId = businessId,
                Remark = remark,
                CreatedUserId = userId
            };

            if (checkoutAmount.HasValue &&
                checkoutAmount > 0)
            {
                entity.SetCheckoutAmount(checkoutAmount.Value);
            }

            await _vodManagerRecordRepository.CreateRecordAsync(entity);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
