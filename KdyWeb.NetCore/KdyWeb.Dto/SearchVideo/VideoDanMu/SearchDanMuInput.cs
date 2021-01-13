using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 弹幕搜索 Input
    /// </summary>
    public class SearchDanMuInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoDanMu.Msg), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
