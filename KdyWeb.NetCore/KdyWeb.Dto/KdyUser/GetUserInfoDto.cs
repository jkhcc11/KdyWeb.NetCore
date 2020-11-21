using AutoMapper;
using KdyWeb.Entity;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 用户信息查询 Dto
    /// </summary>
    [AutoMap(typeof(KdyUser))]
    public class GetUserInfoDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int KdyRoleId { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public KdyRoleDto KdyRole { get; set; }
    }
    
}
