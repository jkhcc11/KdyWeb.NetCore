using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Message;
using KdyWeb.IService.Message;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.Message
{
    /// <summary>
    /// 发送Email服务 实现
    /// </summary>
    public class SendEmailService : BaseKdyService, ISendEmailService
    {
        public SendEmailService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> SendEmailAsync(SendEmailInput input)
        {
            KdyLog.Debug($"邮件发送{input.Email}", new Dictionary<string, object>()
            {
                {"SendInput",input}
            }, nameof(SendEmailAsync));

            var smtpConfig = KdyConfiguration.GetSection(KdyWebServiceConst.SmtpKey).Get<SmtpConfig>();
            if (smtpConfig == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "未配置邮件服务器信息");
            }

            var mess = new MailMessage
            {
                From = new MailAddress(smtpConfig.User),
                Priority = MailPriority.Normal,
                Subject = input.Subject,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                Body = input.Content
            };
            var client = new SmtpClient //new 出客服端的 smtp
            {
                // UseDefaultCredentials = true,//设置为发送认证消息
                Host = smtpConfig.Smtp,
                Credentials = new NetworkCredential(smtpConfig.User, smtpConfig.Pwd),
                Timeout = 5000,
                Port = smtpConfig.Port,
                EnableSsl = smtpConfig.EnableSsl
            };

            mess.To.Add(input.Email);
            try
            {
                await client.SendMailAsync(mess);
                return KdyResult.Success("发送成功");
            }
            catch (Exception ex)
            {
                ex.ToExceptionless()
                    .AddObject(input, "SendInput")
                    .Submit();
                return KdyResult.Error(KdyResultCode.Error, $"发送邮件异常,{ex.Message}");
            }
        }

    }
}
