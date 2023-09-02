using System;
using Kdy.StandardJob.JobInput;
using Kdy.StandardJob.JobService;
using KdyWeb.BaseInterface;
using KdyWeb.Dto.KdyImg;
using KdyWeb.Dto.Message;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.FileStore;
using KdyWeb.IService.Message;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 旧项目通用job管理 实现
    /// </summary>
    public class OldJobService : IOldJobService
    {
        private readonly ILogger<OldJobService> _logger;
        private readonly ISendEmailService _sendEmailService;
        private readonly IKdyImgSaveService _kdyImgSaveService;
        private readonly IDouBanInfoService _douBanInfoService;
        private readonly IFeedBackInfoService _feedBackInfoService;


        public OldJobService(ISendEmailService sendEmailService, IKdyImgSaveService kdyImgSaveService, IDouBanInfoService douBanInfoService, IFeedBackInfoService feedBackInfoService, ILogger<OldJobService> logger)
        {
            _sendEmailService = sendEmailService;
            _kdyImgSaveService = kdyImgSaveService;
            _douBanInfoService = douBanInfoService;
            _feedBackInfoService = feedBackInfoService;
            _logger = logger;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <remarks>
        /// 图片分发上传
        /// </remarks>
        public string UploadImgJob(UploadImgJobInput input)
        {
            var postUrlInput = new PostFileByUrlInput()
            {
                ImgUrl = input.ImgUrl
            };
            var result = KdyAsyncHelper.Run(() => _kdyImgSaveService.PostFileByUrl(postUrlInput));
            _logger.LogDebug("图片上传返回:{result}", result.ToJsonStr());
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
            _logger.LogDebug("发送邮件返回:{result}", result.ToJsonStr());
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
            //  RecurringJob.AddOrUpdate<RecurringUrlJobService>(input.JobId, x => x.Execute(input), input.Cron, TimeZoneInfo.Utc);
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
                    //UserEmail = input.UserEmail, todo:旧项目job
                    Remark = input.Remark
                };

                var createResult = KdyAsyncHelper.Run(() => _feedBackInfoService.CreateFeedBackInfoAsync(feedBackInfo));
                _logger.LogInformation("反馈录入返回:{createResult}", createResult.ToJsonStr());
            }

            _logger.LogInformation("豆瓣信息录入返回:{result}", result.ToJsonStr());
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
                //UserEmail = input.UserEmail, todo:旧项目job
            };
            _feedBackInfoService.CreateFeedBackInfoAsync(feedBackInfo).GetAwaiter();
        }
    }
}
