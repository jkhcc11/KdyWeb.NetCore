using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 分页获取反馈信息 入参
    /// </summary>
    public class GetFeedBackInfoInput : BasePageInput
    {
        /// <summary>
        /// 用户反馈类型
        /// </summary>
        public UserDemandType UserDemandType { get; set; }
    }
}
