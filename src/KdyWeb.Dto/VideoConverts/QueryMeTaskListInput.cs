using KdyWeb.BaseInterface;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询我的任务列表Input
    /// </summary>
    public class QueryMeTaskListInput : QueryConvertTaskWithNormalInput
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        [KdyQuery(nameof(VideoConvertTask.TaskStatus), KdyOperator.Equal)]
        public VideoConvertTaskStatus? TaskStatus { get; set; }
    }
}
