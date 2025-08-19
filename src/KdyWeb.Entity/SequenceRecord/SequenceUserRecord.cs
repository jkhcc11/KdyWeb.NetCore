using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SequenceRecord
{
    /// <summary>
    /// 用户接龙列表
    /// </summary>
    /// <remarks>
    /// 用于记录用户Id初始化
    /// </remarks>
    public class SequenceUserRecord : BaseEntity<long>
    {
        public const int UserNickNameLength = 20;
        public const int UserShowNameLength = 20;
        public const int UserIdLength = 20;

        /// <summary>
        /// 用户接龙列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userNickName">用户昵称</param>
        /// <param name="userShowName">用户显示昵称</param>
        public SequenceUserRecord(string userId, string userNickName, string userShowName)
        {
            UserId = userId;
            UserNickName = userNickName;
            UserShowName = userShowName;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [StringLength(UserIdLength)]
        public string UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        /// <remarks>
        ///  对应微信昵称
        /// </remarks>
        [StringLength(UserNickNameLength)]
        public string UserNickName { get; protected set; }

        /// <summary>
        /// 用户显示昵称
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        [StringLength(UserShowNameLength)]
        public string UserShowName { get; protected set; }
    }
}
