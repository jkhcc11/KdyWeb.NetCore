using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 用户收藏查询 Input
    /// </summary>
    public class QueryUserSubscribeInput : BasePageInput
    {
        /// <summary>
        /// 用户订阅类型
        /// </summary>
        public UserSubscribeType? UserSubscribeType { get; set; }
    }
}
