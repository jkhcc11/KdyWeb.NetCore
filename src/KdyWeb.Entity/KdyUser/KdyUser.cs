using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Utility;

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
        /// 构造
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userNick">用户昵称</param>
        /// <param name="userEmail">邮箱</param>
        /// <param name="kdyRoleId">角色id</param>
        public KdyUser(string userName, string userNick, string userEmail, int kdyRoleId)
        {
            UserName = userName;
            UserNick = userNick;
            UserEmail = userEmail;
            KdyRoleId = kdyRoleId;
        }

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
        [Required]
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
        /// Old用户Id
        /// </summary>
        public int OldUserId { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public KdyRole KdyRole { get; set; }

        /// <summary>
        /// 重置新密码
        /// </summary>
        /// <param name="kdyUser"></param>
        /// <param name="pwd">密码</param>
        public static void SetPwd(KdyUser kdyUser, string pwd)
        {
            kdyUser.UserPwd = $"{pwd}{KdyWebConst.UserSalt}".Md5Ext();
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="userEmail">邮箱</param>
        /// <param name="userNick">昵称</param>
        public void SetUserInfo(string userEmail,string userNick)
        {
            UserEmail = userEmail;
            UserNick = userNick;
        }
    }
}
