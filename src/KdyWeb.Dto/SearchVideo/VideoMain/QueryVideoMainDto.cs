using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 分页查询影视库 Dto
    /// </summary>
    [AutoMap(typeof(VideoMain))]
    public class QueryVideoMainDto : CreatedUserDto<long>
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
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 是否匹配影片信息Url
        /// </summary>
        public bool IsMatchInfo { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 影片状态
        /// </summary>
        public VideoMainStatus VideoMainStatus { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        public string Aka { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 源Url特征码
        /// </summary>
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 影片来源url
        /// </remarks>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 影片主表 扩展信息
        /// </summary>
        public VideoMainInfoDto VideoMainInfo { get; set; }
    }
}
