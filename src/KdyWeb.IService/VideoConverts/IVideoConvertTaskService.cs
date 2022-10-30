using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.VideoConverts;

namespace KdyWeb.IService.VideoConverts
{
    /// <summary>
    /// 视频转码任务 服务接口
    /// </summary>
    public interface IVideoConvertTaskService : IKdyService
    {
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateTaskAsync(CreateTaskInput input);

        /// <summary>
        /// 查询任务列表(admin)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryConvertTaskWithAdminDto>>> QueryConvertTaskWithAdminAsync(QueryConvertTaskWithAdminInput input);

        /// <summary>
        /// 查询任务列表(normal)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryConvertTaskWithNormalDto>>> QueryConvertTaskWithNormalAsync(QueryConvertTaskWithNormalInput input);

        /// <summary>
        /// 接任务
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> TakeTaskAsync(List<long> taskIds);

        /// <summary>
        /// 查询我的任务列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryMeTaskListDto>>> QueryMeTaskListAsync(QueryMeTaskListInput input);

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> SubmitTaskAsync(SubmitTaskInput input);

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CancelTaskAsync(BatchDeleteForLongKeyInput input);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteTaskAsync(long taskId);
    }
}
