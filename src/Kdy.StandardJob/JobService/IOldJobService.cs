﻿using Hangfire;
using Kdy.StandardJob.JobInput;

namespace Kdy.StandardJob.JobService
{
    /// <summary>
    ///  旧项目通用job管理 接口
    /// </summary>
    /// <remarks>
    ///  1、所有旧项目需要执行的非即时后台任务
    /// </remarks>
    public interface IOldJobService : IKdyJobFlag
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <remarks>
        /// 图片分发上传
        /// </remarks>
        string UploadImgJob(UploadImgJobInput input);

        /// <summary>
        /// 邮件发送
        /// </summary>
        void SendEmailJob(SendEmailJobInput input);

        /// <summary>
        /// 添加循环请求Url Job
        /// </summary>
        void RecurringUrlJob(RecurringUrlJobInput input);

        /// <summary>
        /// 豆瓣信息录入Job
        /// </summary>
        void SaveDouBanInfoJob(SaveDouBanInfoInput input);

        /// <summary>
        /// 用户反馈Job
        /// </summary>
        void UserFeedBackJob(UserFeedBackJobInput input);
    }
}
