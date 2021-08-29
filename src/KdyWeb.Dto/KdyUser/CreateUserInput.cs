using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名必填")]
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
        public string UserPwd { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "用户邮箱必填")]
        public string UserEmail { get; set; }
    }
}
