using KdyWeb.BaseInterface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.KdyUser;

namespace KdyWeb.IService.KdyUser
{
    /// <summary>
    /// 消息中心 服务接口
    /// </summary>
    public interface IKdyMsgCenterService : IKdyService
    {
        /// <summary>
        /// 根据用户和类型获取未读Top消息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IReadOnlyList<GetNoReadTopMsgDto>>> GetNoReadTopMsgAsync(GetNoReadTopMsgInput input);

        /// <summary>
        ///已读消息
        /// </summary>
        /// <param name="msgId">消息Id</param>
        /// <returns></returns>
        Task<KdyResult> ReadMsgAsync(long msgId);

        /// <summary>
        /// 检查用户当前过期状态
        /// </summary>
        /// <remarks>
        /// 解析用户正常且即将过期的提醒
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> CheckUserExpiredAsync();
    }
}
