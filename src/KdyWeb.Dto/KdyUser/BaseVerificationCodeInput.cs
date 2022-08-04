using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 验证码基类Input
    /// </summary>
    public interface IBaseVerificationCodeInput
    {
        string EmailCode { get; set; }
    }

    /// <summary>
    /// 邮箱验证码基类
    /// </summary>
    public abstract class BaseEmailInput : IBaseVerificationCodeInput
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "用户邮箱必填")]
        [EmailAddress]
        public string UserEmail { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "验证码异常")]
        public string EmailCode { get; set; }
    }
}
