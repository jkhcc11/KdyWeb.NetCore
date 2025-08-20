using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.SequenceRecord;
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

            foreach (var item in useRecords)
            {
                var dbItem = dbCurrentDateRecords.FirstOrDefault(a => a.UserId == item.UserId);
                if (dbItem == null)
                {
                    //新增
                    createRecords.Add(item);
                }
                else if (dbItem.CurrentPrice != item.CurrentPrice)
                {
                    dbItem.UpdateCurrentPrice(item.GiftUserType, item.CurrentPrice);
                    //修改
                    updateRecords.Add(dbItem);
                }
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
    }
}
