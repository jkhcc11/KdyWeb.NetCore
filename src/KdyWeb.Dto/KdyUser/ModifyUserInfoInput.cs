using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 修改用户信息
    /// </summary>
    public class ModifyUserInfoInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称必填")]
        public string UserNick { get; set; }
    }
}
