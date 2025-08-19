using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.SequenceRecord;
using KdyWeb.Entity.SequenceRecord.Enum;

namespace KdyWeb.IRepository.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置 仓储接口
    /// </summary>
    public interface IGiftUserConfigRepository : IKdyRepository<GiftUserConfig, long>
    {
        /// <summary>
        /// 根据场馆Id获取当前有效奖励配置用户信息
        /// </summary>
        /// <param name="venuesId">场馆Id</param>
        /// <param name="currentDate">当前日期</param>
        /// <returns></returns>
        Task<List<GiftUserConfig>> GetValidGiftUserConfigByVenuesIdAsync(long venuesId, DateTime currentDate);

        /// <summary>
        /// 根据用户Id和奖励用户类型查询是否存在有效配置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="giftUserType">奖励用户类型</param>
        /// <returns></returns>
        Task<bool> ExistByUserIdAndGiftUserTypeAsync(string userId, GiftUserTypeEnum giftUserType);
    }
}
