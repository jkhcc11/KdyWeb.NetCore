using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 注册用户
    /// </summary>
    public class KdyUser : BaseEntity<long>
    {
        #region 常量
        /// <summary>
        /// 用户名长度
        /// </summary>
        public const int UserNameLength = 100;

        /// <summary>
        /// 用户昵称长度
        /// </summary>
        public const int UserNickLength = 50;

        /// <summary>
        /// 邮箱长度
        /// </summary>
        public const int UserEmailLength = 100;

        /// <summary>
        /// 用户密码长度
        /// </summary>
        public const int UserPwdLength = 50;

        /// <summary>
        /// 手机号长度
        /// </summary>
        public const int PhoneNumberLength = 11;
        #endregion

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(UserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(UserNickLength)]
        public string UserNick { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringLength(UserEmailLength)]
        public string UserEmail { get; set; }

        /// <summary>
        ///  密码
        /// </summary>
        [StringLength(UserPwdLength)]
        public string UserPwd { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(PhoneNumberLength)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int KdyRoleId { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public KdyRole KdyRole { get; set; }
    }
}
