using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserInput: BaseEmailInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名必填")]
        [RegularExpression("^[a-zA-Z0-9_@\\-\\.\\+]+$",ErrorMessage = "用户名格式错误,只能由a-z、A-Z、0-9组成")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required(ErrorMessage = "用户昵称必填")]
        public string UserNick { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "用户密码必填")]
        [StringLength(Entity.KdyUser.UserPwdLength, ErrorMessage = "密码长度不能少于8位", MinimumLength = 8)]
        public string UserPwd { get; set; }
    }
}
