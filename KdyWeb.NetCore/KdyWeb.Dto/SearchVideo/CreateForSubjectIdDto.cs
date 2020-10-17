using AutoMapper;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.Utility;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建豆瓣信息Dto
    /// </summary>
    [AutoMap(typeof(DouBanInfo))]
    public class CreateForSubjectIdDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string VideoTitle { get; set; }

        /// <summary>
        /// 豆瓣信息状态
        /// </summary>
        public DouBanInfoStatus DouBanInfoStatus { get; set; }

        /// <summary>
        /// 豆瓣信息状态Str
        /// </summary>
        public string DouBanInfoStatusStr => DouBanInfoStatus.GetDisplayName();

        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 影片类型Str
        /// </summary>
        public string SubtypeStr => Subtype.GetDisplayName();

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }
    }
}
