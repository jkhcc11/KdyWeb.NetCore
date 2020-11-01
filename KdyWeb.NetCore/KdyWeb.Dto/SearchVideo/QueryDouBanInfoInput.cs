using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 豆瓣信息查询 入参
    /// </summary>
    public class QueryDouBanInfoInput : BasePageInput
    {
        /// <summary>
        /// 豆瓣信息状态
        /// </summary>
        [KdyQuery(nameof(DouBanInfo.DouBanInfoStatus), KdyOperator.Equal)]
        public DouBanInfoStatus? DouBanInfoStatus { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(DouBanInfo.VideoTitle), KdyOperator.Like)]
        [KdyQuery(nameof(DouBanInfo.VideoDetailId), KdyOperator.Like)]
        [KdyQuery(nameof(DouBanInfo.VideoYear), KdyOperator.Equal)]
        public string Key { get; set; }
    }
}
