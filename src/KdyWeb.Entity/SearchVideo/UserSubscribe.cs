using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 用户订阅
    /// </summary>
    public class UserSubscribe : BaseEntity<long>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="businessId">业务Id</param>
        /// <param name="businessFeature">业务特征码</param>
        /// <param name="userSubscribeType">用户订阅类型</param>
        public UserSubscribe(long businessId, string businessFeature, UserSubscribeType userSubscribeType)
        {
            BusinessId = businessId;
            BusinessFeature = businessFeature;
            UserSubscribeType = userSubscribeType;
        }

        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        /// 影片Id、小说Id
        /// </remarks>
        public long BusinessId { get; set; }

        /// <summary>
        /// 业务特征码
        /// </summary>
        public string BusinessFeature { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string? UserEmail { get; set; }

        /// <summary>
        /// 用户订阅类型
        /// </summary>
        public UserSubscribeType UserSubscribeType { get; set; }
    }
}
