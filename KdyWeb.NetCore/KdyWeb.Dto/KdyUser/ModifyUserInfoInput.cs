using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 修改用户信息
    /// </summary>
    public class ModifyUserInfoInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称必填")]
        public string UserNick { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱必填")]
        [RegularExpression("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", ErrorMessage = "邮箱错误")]
        public string UserEmail { get; set; }
    }
}
