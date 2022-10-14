using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.VideoConverts;

namespace KdyWeb.IRepository.VideoConverts
{
    /// <summary>
    /// 影片管理者记录 仓储接口
    /// </summary>
    public interface IVodManagerRecordRepository : IKdyRepository<VodManagerRecord, long>
    {
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <returns></returns>
        Task CreateRecordAsync(VodManagerRecord entity);

        /// <summary>
        /// 根据主键批量获取记录
        /// </summary>
        /// <returns></returns>
        Task<List<VodManagerRecord>> BatchGetRecordByIds(List<long> ids);
    }
}
