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
    /// 接龙使用记录 仓储实现
    /// </summary>
    public class SequenceUseRecordRepository : KdyRepository<SequenceUseRecord, long>, ISequenceUseRecordRepository
    {
        public SequenceUseRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 批量创建使用记录
        /// </summary>
        /// <remarks>
        ///  1、如果用户当天不存在使用记录，则直接新增
        ///  2、如果存在则更新价格和类型，以最新为准
        /// </remarks>
        /// <param name="useRecords">用户记录</param> 
        /// <returns></returns>
        public async Task<bool> BatchCreateAsync(List<SequenceUseRecord> useRecords)
        {
            var currentUseRecordsUserIds = useRecords.Select(a => a.UserId).ToArray();
            var totalDate = useRecords.First().UseDate;
            var dbCurrentDateRecords = await DbSet
                .Where(a => a.UseDate == totalDate)
                .ToListAsync();
            if (dbCurrentDateRecords.Any() == false)
            {
                //无记录 直接新增即可
                await CreateAsync(useRecords);
                return true;
            }

            //有记录设计新增、删除、修改
            var updateRecords = new List<SequenceUseRecord>();
            var createRecords = new List<SequenceUseRecord>();

            //需要减少使用次数的用户奖励配置
            var subtractUserGiftConfig = new List<long>();

            foreach (var item in useRecords)
            {
                var dbItem = dbCurrentDateRecords.FirstOrDefault(a => a.UserId == item.UserId);
                if (dbItem == null)
                {
                    //新增
                    createRecords.Add(item);
                }
                else if (dbItem.GiftConfigId != item.GiftConfigId)
                {
                    //如果原来是扣次数的奖励需要还原次数
                    if (dbItem.GiftUserType.HasValue &&
                        dbItem.GiftUserType.Value.IsTotalCount(dbItem.CurrentPrice) &&
                        dbItem.GiftConfigId.HasValue)
                    {
                        subtractUserGiftConfig.Add(dbItem.GiftConfigId.Value);
                    }

                    dbItem.UpdateCurrentPrice(item.GiftUserType, item.CurrentPrice, item.GiftConfigId);
                    //修改
                    updateRecords.Add(dbItem);
                }
            }

            if (subtractUserGiftConfig.Any())
            {
                await SubtractUserGiftConfigAsync(subtractUserGiftConfig);
            }

            //接龙没有，数据库有的就删除
            var deleteRecords = dbCurrentDateRecords
                .Where(a => currentUseRecordsUserIds.Contains(a.UserId) == false)
                .ToList();

            if (createRecords.Any())
            {
                await CreateAsync(createRecords);
            }

            if (updateRecords.Any())
            {
                Update(updateRecords);
            }

            if (deleteRecords.Any())
            {
                Delete(deleteRecords);
            }

            return true;
        }

        /// <summary>
        /// 根据接龙时间获取使用记录
        /// </summary>
        /// <param name="useDate">使用时间</param>
        /// <returns></returns>
        public async Task<List<SequenceUseRecord>> GetSequenceUseRecordByDateAsync(DateTime useDate)
        {
            return await DbSet
                .Where(a => a.UseDate == useDate)
                .ToListAsync();
        }


        /// <summary>
        /// 变更奖励类型
        /// </summary>
        /// <remarks>
        /// 原来有或者无奖励->新的为有奖励
        /// </remarks>
        /// <param name="dbEntity">待变更的记录</param>
        /// <param name="newGiftUserConfig">新的奖励用户配置</param>
        /// <returns></returns>
        public async Task ChangeGiftTypeAsync(SequenceUseRecord dbEntity, GiftUserConfig newGiftUserConfig)
        {
            if (dbEntity.GiftConfigId.HasValue &&
                dbEntity.GiftUserType.HasValue &&
                dbEntity.GiftUserType.Value.IsTotalCount(dbEntity.CurrentPrice))
            {
                //旧奖励减少使用次数
                await SubtractUserGiftConfigAsync(new List<long>()
                {
                    dbEntity.GiftConfigId.Value
                });
            }

            dbEntity.UpdateCurrentPrice(newGiftUserConfig.GiftUserType,
                newGiftUserConfig.GiftPrice,
                newGiftUserConfig.Id);

            var userGiftConfigDbSet = BaseUnitOfWork.GetCurrentDbContext(ReadWrite.Write)
                .Set<GiftUserConfig>();

            if (newGiftUserConfig.GiftUserType.IsTotalCount(newGiftUserConfig.GiftPrice))
            {
                //奖励使用次数加上
                newGiftUserConfig.AddGiftUseCount();
                userGiftConfigDbSet.Update(newGiftUserConfig);
            }

            Update(dbEntity);
        }

        /// <summary>
        /// 变更奖励类型
        /// </summary>
        /// <remarks>
        /// 原来有或者无奖励->新的为无奖励
        /// </remarks>
        /// <param name="dbEntity">待变更的记录</param>
        /// <param name="currentPrice">最新结算价格</param>
        /// <returns></returns>
        public async Task ChangeGiftTypeAsync(SequenceUseRecord dbEntity, decimal currentPrice)
        {
            if (dbEntity.GiftConfigId.HasValue &&
                dbEntity.GiftUserType.HasValue &&
                dbEntity.GiftUserType.Value.IsTotalCount(dbEntity.CurrentPrice))
            {
                //旧奖励减少使用次数
                await SubtractUserGiftConfigAsync(new List<long>()
                {
                    dbEntity.GiftConfigId.Value
                });
            }

            dbEntity.UpdateCurrentPrice(null, currentPrice, null);
        }

        /// <summary>
        /// 减少奖励用户使用次数（用于变更奖励后还原次数）
        /// </summary>
        /// <param name="configIds">奖励用户配置Ids</param>
        /// <returns></returns>
        private async Task SubtractUserGiftConfigAsync(List<long> configIds)
        {
            var userGiftConfigDbSet = BaseUnitOfWork.GetCurrentDbContext(ReadWrite.Write)
                .Set<GiftUserConfig>();
            var dbUserGiftConfig = await userGiftConfigDbSet
                .Where(a => configIds.Contains(a.Id))
                .ToListAsync();
            foreach (var item in dbUserGiftConfig)
            {
                item.SubtractUseCount();
            }

            userGiftConfigDbSet.UpdateRange(dbUserGiftConfig);
        }
    }
}
