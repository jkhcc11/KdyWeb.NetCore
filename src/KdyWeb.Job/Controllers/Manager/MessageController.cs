using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.Message;
using KdyWeb.IService.Message;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 消息发送
    /// </summary>
    public class MessageController : BaseManagerController
    {
        private readonly ISendEmailService _sendEmailService;

        public MessageController(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost("send-email")]
        public async Task<KdyResult> SendEmailAsync(SendEmailInput input)
        {
            var result = await _sendEmailService.SendEmailAsync(input);
            return result;
        }

    }
}
