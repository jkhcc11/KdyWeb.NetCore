using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建用户收藏 Input
    /// </summary>
    public class CreateUserSubscribeInput
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 订阅类型
        /// </summary>
        public UserSubscribeType SubscribeType { get; set; }
    }
}
