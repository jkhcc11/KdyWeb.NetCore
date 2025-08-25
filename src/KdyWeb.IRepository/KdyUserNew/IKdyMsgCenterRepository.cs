using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.KdyUserNew;
using KdyWeb.Entity.KdyUserNew.Enum;

namespace KdyWeb.IRepository.KdyUserNew
{
    /// <summary>
    /// 消息中心 仓储接口
    /// </summary>
    public interface IKdyMsgCenterRepository : IKdyRepository<KdyMsgCenter, long>
    {
        /// <summary>
        /// 根据用户和类型获取未读Top消息
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="topMsg">前n的消息</param>
        /// <returns></returns>
        Task<IReadOnlyList<KdyMsgCenter>> GetNoReadTopMsgAsync(MsgTypeEnum msgType, long userId, int topMsg);
    }
}
