using System;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询我的任务列表Dto
    /// </summary>
    [AutoMap(typeof(VideoConvertTask))]
    public class QueryMeTaskListDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public ConvertTaskType TaskType { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public VideoConvertTaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 任务获取积分
        /// </summary>
        public decimal GiftPoints { get; set; }

        /// <summary>
        /// 资源链接类型
        /// </summary>
        public SourceLinkType SourceLinkType { get; set; }

        /// <summary>
        /// 资源链接
        /// </summary>
        /// <remarks>
        /// todo:后面可能屏蔽
        /// </remarks>
        public string SourceLink { get; set; }

        /// <summary>
        /// 资源链接扩展信息
        /// </summary>
        public string SourceLinkExt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string TaskRemark { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? TakeTime { get; set; }
    }
}
