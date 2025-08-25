using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.KdyUserNew;
using KdyWeb.Entity.KdyUserNew.Enum;
using KdyWeb.IRepository.KdyUserNew;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository.KdyUserNew
{
    /// <summary>
    /// 消息中心 仓储实现
    /// </summary>
    public class KdyMsgCenterRepository : KdyRepository<KdyMsgCenter, long>, IKdyMsgCenterRepository
    {
        public KdyMsgCenterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 根据用户和类型获取未读Top消息
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="topMsg">前n的消息</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<KdyMsgCenter>> GetNoReadTopMsgAsync(MsgTypeEnum msgType, long userId, int topMsg)
        {
            return await DbSet
                .Where(a => a.MsgType == msgType &&
                          a.UserId == userId &&
                          a.IsRead == false)
                .OrderByDescending(a => a.CreatedTime)
                .Take(topMsg)
                .ToListAsync();
        }
    }
}
