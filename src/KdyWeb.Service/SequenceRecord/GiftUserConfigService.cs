using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.IService.SequenceRecord;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SequenceRecord;
using KdyWeb.IRepository.SequenceRecord;
using KdyWeb.BaseInterface;
using System.Collections.Generic;
using System.Linq;
using KdyWeb.Entity.SequenceRecord;
using Microsoft.EntityFrameworkCore;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SequenceRecord.Enum;

namespace KdyWeb.Service.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置 服务实现
    /// </summary>
    public class GiftUserConfigService : BaseKdyService, IGiftUserConfigService
    {
        private readonly IGiftUserConfigRepository _giftUserConfigRepository;
        private readonly IKdyRepository<SequenceUserRecord, long> _sequenceUserRecordRepository;
        private readonly IKdyRepository<VenuesConfig, long> _venuesConfigRepository;

        public GiftUserConfigService(IUnitOfWork unitOfWork, IGiftUserConfigRepository giftUserConfigRepository,
            IKdyRepository<SequenceUserRecord, long> sequenceUserRecordRepository, IKdyRepository<VenuesConfig, long> venuesConfigRepository)
            : base(unitOfWork)
        {
            _giftUserConfigRepository = giftUserConfigRepository;
            _sequenceUserRecordRepository = sequenceUserRecordRepository;
            _venuesConfigRepository = venuesConfigRepository;
        }

        /// <summary>
        /// 分页查询奖励用户配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryPageGiftUserConfigDto>>> QueryPageGiftUserConfigAsync(QueryPageGiftUserConfigInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new()
                {
                    Key = nameof(GiftUserConfig.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _giftUserConfigRepository.GetQuery();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.UserShowName.Contains(input.KeyWord));
            }

            if (input.GiftUserType.HasValue)
            {
                query = query.Where(a => a.GiftUserType == input.GiftUserType.Value);
            }

            var pageList = await query
                .GetDtoPageListAsync<GiftUserConfig, QueryPageGiftUserConfigDto>(input);
            if (pageList.Data != null &&
                pageList.Data.Any())
            {
                //有数据球馆赋值
                var allVenues = await _venuesConfigRepository.GetListAsync(a => a.Id > 0);
                foreach (var item in pageList.Data)
                {
                    item.VenuesName = allVenues.FirstOrDefault(a => a.Id == item.VenuesId)?.VenuesName;
                }
            }

            return KdyResult.Success(pageList, "操作成功");
        }

        /// <summary>
        /// 创建奖励用户配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateGiftUserConfigAsync(CreateGiftUserConfigInput input)
        {
            if (input.GiftStartTime > input.GiftEndTime)
            {
                return KdyResult.Error(KdyResultCode.Error, "开始时间不能大于结束时间");
            }

            if (input.GiftUserType.IsTotalCount(input.GiftPrice) &&
                input.GiftValidCount <= 0)
            {
                return KdyResult.Error(KdyResultCode.Error, "擂主和奖励免卡需要输入次数");
            }

            if (await _venuesConfigRepository.GetQuery().AnyAsync(a => a.Id == input.VenuesId) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "场馆不存在");
            }

            if (await _giftUserConfigRepository.ExistByUserIdAndGiftUserTypeAsync(input.UserId, input.GiftUserType, input.VenuesId))
            {
                return KdyResult.Error(KdyResultCode.Duplicate, "当前用户已存在该类型配置");
            }

            //用户使用记录
            var userRecord = await _sequenceUserRecordRepository.GetQuery()
                .Where(a => a.UserId == input.UserId)
                .Select(a => new
                {
                    a.UserId,
                    a.UserShowName
                })
                .FirstOrDefaultAsync();
            if (userRecord == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "用户不存在，请先同步记录");
            }

            var entity = new GiftUserConfig(input.UserId, input.GiftUserType,
                input.GiftStartTime, input.GiftEndTime)
            {
                GiftPrice = input.GiftPrice,
                VenuesId = input.VenuesId,
                Remark = input.Remark,
                UserShowName = userRecord.UserShowName,
                GiftValidCount = input.GiftValidCount
            };
            await _giftUserConfigRepository.CreateAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改奖励用户配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyGiftUserConfigAsync(ModifyGiftUserConfigInput input)
        {
            var dbEntity = await _giftUserConfigRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            if (dbEntity.GiftEndTime != input.GiftEndTime)
            {
                //只有改了时间重置次数
                dbEntity.InitGiftUseCount();
            }

            dbEntity.SetGiftEndTime(input.GiftEndTime);
            dbEntity.SetGiftStartTime(input.GiftStartTime);
            dbEntity.Remark = input.Remark;
            dbEntity.GiftPrice = input.GiftPrice;
            dbEntity.GiftValidCount = input.GiftValidCount;

            _giftUserConfigRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 禁用奖励用户配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BanGiftUserConfigAsync(long configId)
        {
            var dbEntity = await _giftUserConfigRepository.FirstOrDefaultAsync(a => a.Id == configId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            if (dbEntity.IsEnable)
            {
                dbEntity.Ban();
            }
            else
            {
                dbEntity.Enable();
            }

            _giftUserConfigRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 删除奖励用户配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(long configId)
        {
            var dbEntity = await _giftUserConfigRepository.FirstOrDefaultAsync(a => a.Id == configId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            _giftUserConfigRepository.Delete(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
