using System;
using Hangfire;
using Kdy.StandardJob.JobInput;
using Kdy.StandardJob.JobService;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto.Message;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.Message;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 旧项目通用job管理 实现
    /// </summary>
    public class OldJobService : IOldJobService
    {
        private readonly IKdyLog _kdyLog;
        private readonly ISendEmailService _sendEmailService;
        private readonly IKdyImgSaveService _kdyImgSaveService;
        private readonly IDouBanInfoService _douBanInfoService;
        private readonly IFeedBackInfoService _feedBackInfoService;


        public OldJobService(IKdyLog kdyLog, ISendEmailService sendEmailService, IKdyImgSaveService kdyImgSaveService, IDouBanInfoService douBanInfoService, IFeedBackInfoService feedBackInfoService)
        {
            _kdyLog = kdyLog;
            _sendEmailService = sendEmailService;
            _kdyImgSaveService = kdyImgSaveService;
            _douBanInfoService = douBanInfoService;
            _feedBackInfoService = feedBackInfoService;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <remarks>
        /// 图片分发上传
        /// </remarks>
        public string UploadImgJob(UploadImgJobInput input)
        {
            var result = KdyAsyncHelper.Run(() => _kdyImgSaveService.PostFileByUrl(input.ImgUrl));
            _kdyLog.Debug($"图片上传返回{result.ToJsonStr()}");
            if (result.IsSuccess == false)
            {
                return string.Empty;
            }

            return result.Data;
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        public void SendEmailJob(SendEmailJobInput input)
        {
            var sendInput = new SendEmailInput(input.Email, input.Subject, input.Content);
            var result = KdyAsyncHelper.Run(() => _sendEmailService.SendEmailAsync(sendInput));
            _kdyLog.Debug($"发送邮件返回{result.ToJsonStr()}");
            if (result.IsSuccess == false)
            {
                throw new Exception(result.Msg);
            }
        }

        /// <summary>
        /// 添加循环请求Url Job
        /// </summary>
        public void RecurringUrlJob(RecurringUrlJobInput input)
        {
            RecurringJob.AddOrUpdate<RecurringUrlJobService>(input.JobId, x => x.Execute(input), input.Cron, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// 豆瓣信息录入Job
        /// </summary>
        public void SaveDouBanInfoJob(SaveDouBanInfoInput input)
        {
            var result = KdyAsyncHelper.Run(() => _douBanInfoService.CreateForSubjectIdAsync(input.SubjectId));
            if (result.IsSuccess)
            {
                var url = $"//movie.douban.com/subject/{input.SubjectId}/";
                var feedBackInfo = new CreateFeedBackInfoInput()
                {
                    VideoName = result.Data.VideoTitle,
                    OriginalUrl = url,
                    DemandType = UserDemandType.Input,
                    UserEmail = input.UserEmail
                };

                var createResult = KdyAsyncHelper.Run(() => _feedBackInfoService.CreateFeedBackInfoAsync(feedBackInfo));
                _kdyLog.Info($"反馈录入返回:{createResult.ToJsonStr()}");
            }

            _kdyLog.Info($"豆瓣信息录入返回:{result.ToJsonStr()}");
        }

        /// <summary>
        /// 用户反馈Job
        /// </summary>
        public void UserFeedBackJob(UserFeedBackJobInput input)
        {
            var feedBackInfo = new CreateFeedBackInfoInput()
            {
                VideoName = input.VideoName,
                OriginalUrl = input.Url,
                DemandType = UserDemandType.Feedback,
                UserEmail = input.UserEmail
            };
            _feedBackInfoService.CreateFeedBackInfoAsync(feedBackInfo).GetAwaiter();
        }
    }
}
