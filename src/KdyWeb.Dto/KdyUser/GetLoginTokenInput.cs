using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.KdyUser
{
    /// <summary>
    /// 获取用户登录Token Input
    /// </summary>
    public class GetLoginTokenInput
    {
        /// <summary>
        /// 用户名或邮箱
        /// </summary>
        [Required(ErrorMessage = "用户名必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        [StringLength(Entity.KdyUser.UserPwdLength, MinimumLength = 8, ErrorMessage = "密码长度不正确")]
        public string UserPwd { get; set; }
    }
}
