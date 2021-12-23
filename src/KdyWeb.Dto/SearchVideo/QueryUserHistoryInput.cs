using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 用户播放记录分页查询 Input
    /// </summary>
    public class QueryUserHistoryInput : BasePageInput
    {
        /// <summary>
        /// 用户订阅类型
        /// </summary>
        public UserSubscribeType? UserSubscribeType { get; set; }
    }
}
