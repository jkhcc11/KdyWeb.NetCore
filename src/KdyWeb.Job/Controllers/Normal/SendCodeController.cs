using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.Message;
using KdyWeb.IService.Message;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Normal
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class SendCodeController : BaseNormalController
    {
        private readonly ISendEmailService _sendEmailService;

        public SendCodeController(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        /// <summary>
        /// 邮箱验证码发送
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendCodeByEmail")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLoginTokenAsync(SendEmailCodeInput input)
        {
            var result = await _sendEmailService.SendEmailCodeAsync(input);
            return Ok(result);
        }
    }
}
