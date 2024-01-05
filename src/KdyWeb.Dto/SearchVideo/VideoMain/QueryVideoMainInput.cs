using System;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 分页查询影视库 Input
    /// </summary>
    public class QueryVideoMainInput : BasePageInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [KdyQuery(nameof(VideoMain.CreatedTime), KdyOperator.GtEqual)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [KdyQuery(nameof(VideoMain.CreatedTime), KdyOperator.LessEqual)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 影片类型
        /// </summary>
        [KdyQuery(nameof(VideoMain.Subtype), KdyOperator.Equal)]
        public Subtype? Subtype { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [KdyQuery(nameof(VideoMain.VideoYear), KdyOperator.Equal)]
        public int? Year { get; set; }

        /// <summary>
        /// 类型 动作，喜剧等
        /// </summary>
        [KdyQuery("VideoMainInfo.VideoGenres", KdyOperator.Like)]
        public string Genres { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(VideoMain.KeyWord), KdyOperator.Like)]
        [KdyQuery(nameof(VideoMain.Aka), KdyOperator.Like)]
        [KdyQuery(nameof(VideoMain.SourceUrl), KdyOperator.StartsWith)]
        public string KeyWord { get; set; }

        /// <summary>
        /// 演员
        /// </summary>
        [KdyQuery("VideoMainInfo.VideoCasts", KdyOperator.Like)]
        public string VideoCasts { get; set; }

        /// <summary>
        /// 搜索类型
        /// </summary>
        public SearchType? SearchType { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public VideoCountries? VideoCountries { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [KdyQuery(nameof(VideoMain.VideoMainStatus), KdyOperator.Equal)]
        public VideoMainStatus? VideoMainStatus { get; set; }
    }

    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// 未完结
        /// </summary>
        IsNoEnd = 1,

        /// <summary>
        /// 当天更新
        /// </summary>
        IsToday,

        /// <summary>
        /// 未匹配豆瓣
        /// </summary>
        IsNoMatchDouBan,

        /// <summary>
        /// 已关联解说
        /// </summary>
        IsNarrateUrl,

        /// <summary>
        /// 低分影片
        /// </summary>
        LowScore,

        /// <summary>
        /// 待维护资源
        /// </summary>
        ToBeMaintained
    }

    /// <summary>
    /// 国家
    /// </summary>
    public enum VideoCountries
    {
        /// <summary>
        /// 美剧
        /// </summary>
        [Display(Name = "美国")]
        American = 1,

        /// <summary>
        /// 英剧
        /// </summary>
        [Display(Name = "英国")]
        EnglishDrama = 2,

        /// <summary>
        /// 韩剧
        /// </summary>
        [Display(Name = "韩国")]
        KoreanDrama = 3,

        /// <summary>
        /// 日剧
        /// </summary>
        [Display(Name = "日本")]
        JapaneseOpera = 4,

        /// <summary>
        /// 港剧
        /// </summary>
        [Display(Name = "中国香港")]
        HK = 5,

        /// <summary>
        /// 泰剧
        /// </summary>
        [Display(Name = "泰国")]
        ThaiOpera = 6,

        /// <summary>
        /// 国产剧
        /// </summary>
        [Display(Name = "中国大陆")]
        China = 7,

        /// <summary>
        /// 其他
        /// </summary>
        [Display(Name = "其他")]
        Other = 8
    }
}
