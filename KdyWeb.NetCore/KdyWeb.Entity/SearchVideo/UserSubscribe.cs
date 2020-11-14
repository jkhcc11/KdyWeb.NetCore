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
        public UserSubscribe(long businessId, string businessFeature)
        {
            BusinessId = businessId;
            BusinessFeature = businessFeature;
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
        public string UserEmail { get; set; }
    }
}
