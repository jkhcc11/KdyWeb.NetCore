using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.SequenceRecord;
using KdyWeb.Entity.SequenceRecord.Enum;
using KdyWeb.IRepository.SequenceRecord;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置 仓储实现
    /// </summary>
    public class GiftUserConfigRepository : KdyRepository<GiftUserConfig, long>, IGiftUserConfigRepository
    {
        public GiftUserConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 根据场馆Id获取当前有效奖励配置用户信息
        /// </summary>
        /// <param name="venuesId">场馆Id</param>
        /// <param name="currentDate">当前日期</param>
        /// <returns></returns>
        public async Task<List<GiftUserConfig>> GetValidGiftUserConfigByVenuesIdAsync(long venuesId, DateTime currentDate)
        {
            return await DbSet
                .Where(a => a.VenuesId == venuesId &&
                            a.IsEnable &&
                            currentDate >= a.GiftStartTime &&
                            currentDate <= a.GiftEndTime)
                .ToListAsync();
        }

        /// <summary>
        /// 根据用户Id和奖励用户类型查询是否存在有效配置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="giftUserType">奖励用户类型</param>
        /// <param name="venuesId">球馆Id</param>
        /// <returns></returns>
        public async Task<bool> ExistByUserIdAndGiftUserTypeAsync(string userId, GiftUserTypeEnum giftUserType, long venuesId)
        {
            return await DbSet
                .AnyAsync(a => a.UserId == userId &&
                               a.GiftUserType == giftUserType &&
                               a.VenuesId == venuesId);
        }

        /// <summary>
        /// 根据用户Id和奖励用户类型查询是否存在有效配置
        /// </summary>
        /// <param name="totalDate">结算日期</param>
        /// <param name="userId">用户Id</param>
        /// <param name="giftUserType">奖励用户类型</param>
        /// <param name="venuesId">球馆Id</param>
        /// <param name="giftPrice">奖励价格</param>
        /// <returns></returns>
        public async Task<GiftUserConfig?> GetGiftUserConfigByTotalDateAsync(DateTime totalDate, string userId, GiftUserTypeEnum giftUserType, long venuesId,
            decimal giftPrice)
        {
            return await DbSet
                .FirstOrDefaultAsync(a => a.UserId == userId &&
                               a.GiftUserType == giftUserType &&
                               a.VenuesId == venuesId &&
                               a.GiftPrice == giftPrice &&
                               a.GiftStartTime <= totalDate &&
                               totalDate <= a.GiftEndTime);
        }
    }
}
