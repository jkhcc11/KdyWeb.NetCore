using System.ComponentModel.DataAnnotations;
using KdyWeb.Dto.VerificationCode;

namespace KdyWeb.Dto.Message
{
    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <returns></returns>
    public class SendEmailCodeInput
    {
        /// <summary>
        /// 验证码类型
        /// </summary>
        public VerificationCodeType CodeType { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}
