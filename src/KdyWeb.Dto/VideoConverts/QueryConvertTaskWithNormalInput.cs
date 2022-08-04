using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询任务列表 input
    /// </summary>
    public class QueryConvertTaskWithNormalInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoConvertTask.TaskName), KdyOperator.Like)]
        public string KeyWord { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [KdyQuery(nameof(VideoConvertTask.TaskType), KdyOperator.Equal)]
        public ConvertTaskType? TaskType { get; set; }

    }
}
