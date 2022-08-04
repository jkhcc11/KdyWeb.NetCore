using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.VideoConverts
{
    /// <summary>
    /// 转换订单详情
    /// </summary>
    public class ConvertOrderDetail : BaseEntity<long>
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        [StringLength(VideoConvertTask.TaskNameLength)]
        public string TaskName { get; set; }
    }
}
