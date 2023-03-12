using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片主表
    /// </summary>
    public class VideoMain : BaseEntity<long>
    {
        /// <summary>
        /// 手动修复标识
        /// </summary>
        public const string SystemInput = "systeminput";

        /// <summary>
        /// Url统一长度
        /// </summary>
        public const int UrlLength = 280;
        /// <summary>
        /// 源Url特征码
        /// </summary>
        public const int VideoContentFeatureLength = 32;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="subtype">影片类型</param>
        /// <param name="keyWord">视频名称</param>
        /// <param name="videoImg">海报</param>
        /// <param name="sourceUrl">源Url</param>
        /// <param name="videoContentFeature">源Url特征码</param>
        public VideoMain(Subtype subtype, string keyWord, string videoImg, string sourceUrl, string videoContentFeature)
        {
            Subtype = subtype;
            KeyWord = keyWord;
            VideoImg = videoImg;
            SourceUrl = sourceUrl;
            VideoContentFeature = videoContentFeature;
            VideoMainStatus = VideoMainStatus.Normal;

        }

        /// <summary>
        /// 影片类型
        /// </summary>
        [Required]
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越展示靠前
        /// </remarks>
        [Required]
        public int OrderBy { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        [Required]
        public bool IsEnd { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        [StringLength(DouBanInfo.VideoTitleLength)]
        [Required]
        public string KeyWord { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [StringLength(DouBanInfo.VideoImgLength)]
        [Required]
        public string VideoImg { get; set; }

        /// <summary>
        /// 是否匹配影片信息Url
        /// </summary>
        [Required]
        public bool IsMatchInfo { get; set; }

        /// <summary>
        /// 影片状态
        /// </summary>
        [Required]
        public VideoMainStatus VideoMainStatus { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        [StringLength(DouBanInfo.AkaLength)]
        public string Aka { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 影片来源url
        /// </remarks>
        [StringLength(UrlLength)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 源Url特征码
        /// </summary>
        [StringLength(VideoContentFeatureLength)]
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 影片信息Url
        /// </summary>
        /// <remarks>
        /// 豆瓣Url或其他影片介绍地址
        /// </remarks>
        [StringLength(UrlLength)]
        public string VideoInfoUrl { get; set; }

        /// <summary>
        /// 旧KeyId
        /// </summary>
        public int OldKeyId { get; set; }

        /// <summary>
        /// 影片主表 扩展信息
        /// </summary>
        public virtual VideoMainInfo VideoMainInfo { get; set; }

        /// <summary>
        /// 剧集信息组
        /// </summary>
        public virtual ICollection<VideoEpisodeGroup> EpisodeGroup { get; set; }

        /// <summary>
        /// 设置豆瓣信息匹配
        /// </summary>
        public void SetMatchDouBanInfo()
        {
            IsMatchInfo = true;
        }

        /// <summary>
        /// 设置下架
        /// </summary>
        public void SetDown()
        {
            VideoContentFeature = SystemInput;
            SourceUrl = SystemInput;
            IsEnd = true;
            VideoMainStatus = VideoMainStatus.Down;
        }

        /// <summary>
        /// 设置系统录入
        /// </summary>
        public void SetSysInput(bool isEnd)
        {
            VideoContentFeature = SystemInput;
            SourceUrl = SystemInput;
            IsEnd = isEnd;
        }
    }
}
