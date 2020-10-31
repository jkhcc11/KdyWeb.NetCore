using System.Collections.Generic;
using KdyWeb.BaseInterface;
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
        [KdyQuery(nameof(FeedBackInfo.DemandType), KdyOperator.Equal)]
        public UserDemandType? UserDemandType { get; set; }

        /// <summary>
        /// 用户反馈类型
        /// </summary>
        [KdyQuery(nameof(FeedBackInfo.FeedBackInfoStatus), KdyOperator.Equal)]
        public FeedBackInfoStatus? FeedBackInfoStatus { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(FeedBackInfo.UserEmail), KdyOperator.Like)]
        [KdyQuery(nameof(FeedBackInfo.Remark), KdyOperator.Like)]
        [KdyQuery(nameof(FeedBackInfo.VideoName), KdyOperator.Like)]
        [KdyQuery(nameof(FeedBackInfo.OriginalUrl), KdyOperator.Like)]
        public string Key { get; set; }
    }
}
