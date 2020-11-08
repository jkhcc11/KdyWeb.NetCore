using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 影片主表扩展信息
    /// </summary>
    public class VideoMainInfo : BaseEntity<long>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mainId">影片主表Id</param>
        public VideoMainInfo(long mainId)
        {
            MainId = mainId;
        }

        /// <summary>
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        [StringLength(DouBanInfo.VideoGenresLength)]
        public string VideoGenres { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string VideoSummary { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        [StringLength(DouBanInfo.VideoCastsLength)]
        public string VideoCasts { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        [StringLength(DouBanInfo.VideoDirectorsLength)]
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        [StringLength(DouBanInfo.VideoCountriesLength)]
        public string VideoCountries { get; set; }

        /// <summary>
        /// 解说Url
        /// </summary>
        [StringLength(VideoMain.UrlLength)]
        public string NarrateUrl { get; set; }

        /// <summary>
        /// 版权跳转Url
        /// </summary>
        [StringLength(VideoMain.UrlLength)]
        public string BanVideoJumpUrl { get; set; }

        /// <summary>
        /// 影片主表Id
        /// </summary>
        public long MainId { get; set; }

        /// <summary>
        /// 影片主表
        /// </summary>
        public virtual VideoMain VideoMain { get; set; }
    }
}
