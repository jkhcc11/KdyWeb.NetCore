using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.IRepository.VideoConverts;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository.VideoConverts
{
    /// <summary>
    /// 影片管理者记录 仓储实现
    /// </summary>
    public class VodManagerRecordRepository : KdyRepository<VodManagerRecord, long>, IVodManagerRecordRepository
    {
        public VodManagerRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <returns></returns>
        public async Task CreateRecordAsync(VodManagerRecord entity)
        {
            var userId = entity.CreatedUserId;
            if (userId.HasValue == false)
            {
                //没赋值 取登录
                userId = LoginUserInfo.GetUserId();
            }

            var any = await WriteDbSet.AnyAsync(a => a.CreatedUserId == userId &&
                                                     a.RecordType == entity.RecordType &&
                                                     a.BusinessId == entity.BusinessId);
            if (any)
            {
                entity.IsValid = false;
            }

            await WriteDbSet.AddAsync(entity);
        }

        /// <summary>
        /// 根据主键批量获取记录
        /// </summary>
        /// <returns></returns>
        public async Task<List<VodManagerRecord>> BatchGetRecordByIds(List<long> ids)
        {
            return await DbSet.Where(a => ids.Contains(a.Id)).ToListAsync();
        }
    }
}
