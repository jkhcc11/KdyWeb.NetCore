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
        /// <param name="useRecords">用户记录</param> 
        /// <returns></returns>
        public async Task<bool> BatchCreateAsync(List<SequenceUseRecord> useRecords)
        {
            await CreateAsync(useRecords);
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
