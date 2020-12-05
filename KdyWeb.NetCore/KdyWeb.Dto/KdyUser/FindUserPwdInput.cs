using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 找回密码 Input
    /// </summary>
    public class FindUserPwdInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码必填")]
        [StringLength(KdyUser.UserPwdLength, ErrorMessage = "新密码太长")]
        public string NewPwd { get; set; }
    }
}
