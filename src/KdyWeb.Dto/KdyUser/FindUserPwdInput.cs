using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 找回密码 Input
    /// </summary>
    public class FindUserPwdInput : BaseEmailInput
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码必填")]
        [StringLength(Entity.KdyUser.UserPwdLength, ErrorMessage = "密码长度不能少于8位", MinimumLength = 8)]
        public string NewPwd { get; set; }
    }
}
