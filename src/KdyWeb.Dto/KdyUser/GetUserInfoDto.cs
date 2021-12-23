using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 用户信息查询 Dto
    /// </summary>
    [AutoMap(typeof(Entity.KdyUser))]
    public class GetUserInfoDto : BaseEntityDto<long>
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
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName  { get; set; }
    }

}
