using KdyWeb.BaseInterface;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 用户信息查询 Input
    /// </summary>
    public class GetUserInfoInput
    {
        /// <summary>
        /// 邮箱或用户名
        /// </summary>
        [KdyQuery(nameof(Entity.KdyUser.UserEmail), KdyOperator.Equal)]
        [KdyQuery(nameof(Entity.KdyUser.UserName), KdyOperator.Equal)]
        public string UserInfo { get; set; }

        /// <summary>
        /// 旧用户Id
        /// </summary>
        [KdyQuery(nameof(Entity.KdyUser.OldUserId), KdyOperator.Equal)]
        public int OldUserId { get; set; }
    }
}
