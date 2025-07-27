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
        /// <param name="useRecords">用户记录</param>
        /// <returns></returns>
        Task<bool> BatchCreateAsync(List<SequenceUseRecord> useRecords);
    }
}
