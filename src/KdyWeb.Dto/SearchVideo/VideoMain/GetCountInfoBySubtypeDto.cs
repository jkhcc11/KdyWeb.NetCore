using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 获取影片统计信息Dto
    /// </summary>
    public class GetCountInfoBySubtypeDto
    {
        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 影片类型Str
        /// </summary>
        public string SubtypeStr => Subtype.GetDisplayName();

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
}
