using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 修改用户密码 Input
    /// </summary>
    public class ModifyUserPwdInput : IBaseVerificationCodeInput
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码必填")]
        [StringLength(Entity.KdyUser.UserPwdLength, ErrorMessage = "新密码太长", MinimumLength = 8)]
        public string NewPwd { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "验证码异常")]
        public string EmailCode { get; set; }
    }
}
