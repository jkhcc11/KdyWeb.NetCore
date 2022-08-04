using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Message;
using KdyWeb.Dto.VerificationCode;
using KdyWeb.IService.Message;
using KdyWeb.IService.VerificationCode;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KdyWeb.Service.Message
{
    /// <summary>
    /// 发送Email服务 实现
    /// </summary>
    public class SendEmailService : BaseKdyService, ISendEmailService
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IDistributedCache _distributedCache;
        public SendEmailService(IUnitOfWork unitOfWork, IVerificationCodeService verificationCodeService,
             IDistributedCache distributedCache) : base(unitOfWork)
        {
            _verificationCodeService = verificationCodeService;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> SendEmailAsync(SendEmailInput input)
        {
            KdyLog.LogInformation("开始邮件发送.{input}", JsonConvert.SerializeObject(input));

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

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> SendEmailCodeAsync(SendEmailCodeInput input)
        {
            if (input.CodeType == VerificationCodeType.ModifyPwd)
            {
                input.Email = LoginUserInfo.UserEmail;
            }

            if (string.IsNullOrWhiteSpace(input.Email))
            {
                return KdyResult.Error(KdyResultCode.ParError, "邮箱必填");
            }

            var code = await _verificationCodeService.GeneralCodeAsync();
            var emailInput = new SendEmailInput(input.Email, input.CodeType.GetVerificationCodeEmailSubject(), input.CodeType.GetVerificationCodeEmailContent(code));
            var result = await SendEmailAsync(emailInput);
            if (result.IsSuccess == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "验证码发送失败,请重试或联系管理员");
            }

            await _distributedCache.SetStringAsync(input.CodeType.GetVerificationCodeCacheKey(input.Email), code, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return KdyResult.Success(msg: "验证码已发送成功，请前往您的邮箱查看");
        }
    }
}
