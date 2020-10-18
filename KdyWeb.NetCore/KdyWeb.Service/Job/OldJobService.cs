using System;
using Kdy.StandardJob;
using Kdy.StandardJob.JobInput;
using Kdy.StandardJob.JobService;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Message;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.Message;
using KdyWeb.Utility;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 图片上传 实现
    /// </summary>
    public class OldJobService : IOldJobService
    {
        private readonly IKdyLog _kdyLog;
        private readonly ISendEmailService _sendEmailService;
        private readonly IKdyImgSaveService _kdyImgSaveService;

        public OldJobService(IKdyLog kdyLog, ISendEmailService sendEmailService, IKdyImgSaveService kdyImgSaveService)
        {
            _kdyLog = kdyLog;
            _sendEmailService = sendEmailService;
            _kdyImgSaveService = kdyImgSaveService;
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
            var sendInput=new SendEmailInput(input.Email, input.Subject, input.Content);
            var result = KdyAsyncHelper.Run(() => _sendEmailService.SendEmailAsync(sendInput));
            _kdyLog.Debug($"发送邮件返回{result.ToJsonStr()}");
            if (result.IsSuccess == false)
            {
                throw new Exception(result.Msg);
            }
        }
    }
}
