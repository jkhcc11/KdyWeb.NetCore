using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Utility;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘用户
    /// </summary>
    public class CloudParseUser : BaseEntity<long>
    {
        /// <summary>
        /// 自有api地址长度
        /// </summary>
        public const int SelfApiUrlLength = 150;
        /// <summary>
        /// QQ长度
        /// </summary>
        public const int UserQqLength = 15;

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(KdyUser.UserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(KdyUser.UserNickLength)]
        public string UserNick { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(KdyUser.UserEmailLength)]
        public string UserEmail { get; set; }

        /// <summary>
        /// Qq号
        /// </summary>
        [StringLength(UserQqLength)]
        public string UserQq { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(KdyUser.UserPwdLength)]
        public string UserPwd { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public KdyUserStatus UserStatus { get; set; }

        /// <summary>
        /// 自有Api地址
        /// </summary>
        [StringLength(SelfApiUrlLength)]
        public string SelfApiUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[] HoldLinkHost { get; set; }

        /// <summary>
        /// 子账号列表
        /// </summary>
        public virtual ICollection<CloudParseUserChildren> CloudParseUserChildrens { get; set; }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="userRawPwd">原始密码</param>
        /// <returns></returns>
        public bool CheckPwd(string userRawPwd)
        {
            var inputPwd = userRawPwd.Md5Ext();
            return UserPwd == inputPwd;
        }
    }
}
