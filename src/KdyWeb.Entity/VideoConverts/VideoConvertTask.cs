using System;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.VideoConverts.Enum;

namespace KdyWeb.Entity.VideoConverts
{
    /// <summary>
    /// 视频转码任务
    /// </summary>
    public class VideoConvertTask : BaseEntity<long>, IRowVersion
    {
        public const int TakeUserNameLength = 50;
        public const int SourceLinkLength = 500;
        public const int SourceLinkExtLength = 50;
        public const int TaskRemarkLength = 500;
        public const int TaskNameLength = 50;

        /// <summary>
        /// 视频转码任务
        /// </summary>
        /// <param name="taskName">任务名</param>
        /// <param name="taskType">任务类型</param>
        /// <param name="giftPoints">任务获取积分</param>
        /// <param name="sourceLinkType">资源链接类型</param>
        /// <param name="sourceLink">资源链接</param>
        public VideoConvertTask(string taskName, ConvertTaskType taskType,
            decimal giftPoints, SourceLinkType sourceLinkType,
            string sourceLink)
        {
            TaskName = taskName;
            TaskType = taskType;
            GiftPoints = giftPoints;
            SourceLinkType = sourceLinkType;
            SourceLink = sourceLink;
            TaskStatus = VideoConvertTaskStatus.Waiting;
        }

        /// <summary>
        /// 任务名
        /// </summary>
        [StringLength(TaskNameLength)]
        public string TaskName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public ConvertTaskType TaskType { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public VideoConvertTaskStatus TaskStatus { get; protected set; }

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
        [StringLength(SourceLinkLength)]
        public string SourceLink { get; set; }

        /// <summary>
        /// 资源链接扩展信息
        /// </summary>
        /// <remarks>
        /// 提取码等
        /// </remarks>
        [StringLength(SourceLinkExtLength)]
        public string SourceLinkExt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(TaskRemarkLength)]
        public string TaskRemark { get; set; }

        /// <summary>
        /// 接单人id
        /// </summary>
        public long? TakeUserId { get; set; }

        /// <summary>
        /// 接单人
        /// </summary>
        [StringLength(TakeUserNameLength)]
        public string TakeUserName { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? TakeTime { get; protected set; }

        public byte[] RowVersion { get; set; }

        /// <summary>
        /// 接单
        /// </summary>
        public void TakeTask(long userId, string userName)
        {
            TaskStatus = VideoConvertTaskStatus.Padding;
            TakeUserId = userId;
            TakeUserName = userName;
            TakeTime = DateTime.Now;
        }

        /// <summary>
        /// 驳回
        /// </summary>
        public void SetRejected()
        {
            TaskStatus = VideoConvertTaskStatus.Padding;
        }

        /// <summary>
        /// 取消接单
        /// </summary>
        public void CancelTask()
        {
            TaskStatus = VideoConvertTaskStatus.Waiting;
            TakeUserName = string.Empty;
            TakeUserId = null;
            TakeTime = null;
        }

        /// <summary>
        /// 设置审核中
        /// </summary>
        public void SetAuditing()
        {
            TaskStatus = VideoConvertTaskStatus.Auditing;
        }

        /// <summary>
        /// 设置已完成
        /// </summary>
        public void SetFinish()
        {
            TaskStatus = VideoConvertTaskStatus.Finish;
        }
    }
}
