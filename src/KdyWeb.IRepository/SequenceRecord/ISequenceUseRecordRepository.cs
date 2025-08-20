using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.IRepository.SequenceRecord
{
    /// <summary>
    /// 接龙使用记录 仓储接口
    /// </summary>
    public interface ISequenceUseRecordRepository : IKdyRepository<SequenceUseRecord, long>
    {
        /// <summary>
        /// 批量创建使用记录
        /// </summary>
        /// <remarks>
        ///  1、如果用户当天不存在使用记录，则直接新增
        ///  2、如果存在则更新价格和类型，以最新为准
        /// </remarks>
        /// <param name="useRecords">用户记录</param> 
        /// <returns></returns>
        Task<bool> BatchCreateAsync(List<SequenceUseRecord> useRecords);

        /// <summary>
        /// 根据接龙时间获取使用记录
        /// </summary>
        /// <param name="useDate">使用时间</param>
        /// <returns></returns>
        Task<List<SequenceUseRecord>> GetSequenceUseRecordByDateAsync(DateTime useDate);
    }
}
