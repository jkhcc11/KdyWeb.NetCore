using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using Newtonsoft.Json;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片下载地址
    /// </summary>
    public class VideoDownInfo : BaseEntity<long>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="videoName">影片名</param>
        /// <param name="videoImg">海报</param>
        /// <param name="downJson">下载地址</param>
        /// <param name="urlFeature">特征码</param>
        /// <param name="sourceUrl">源Url</param>
        public VideoDownInfo(string videoName, string videoImg, string downJson, string urlFeature, string sourceUrl)
        {
            VideoName = videoName;
            VideoImg = videoImg;
            DownJson = downJson;
            UrlFeature = urlFeature;
            SourceUrl = sourceUrl;
        }

        /// <summary>
        /// 影片名
        /// </summary>
        [StringLength(DouBanInfo.VideoTitleLength)]
        public string VideoName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [StringLength(DouBanInfo.VideoImgLength)]
        public string VideoImg { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        [StringLength(DouBanInfo.VideoDirectorsLength)]
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        /// 序列化<seealso cref="DownUrlJsonItem"/>对象
        public string DownJson { get; set; }

        /// <summary>
        /// 特征码
        /// </summary>
        [StringLength(VideoMain.VideoContentFeatureLength)]
        public string UrlFeature { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        [StringLength(VideoMain.UrlLength)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }
    }

    /// <summary>
    /// 下载地址Item
    /// </summary>
    public class DownUrlJsonItem
    {
        /// <summary>
        /// 下载名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 下载链接
        /// </summary>
        public string Value { get; set; }
    }
}
