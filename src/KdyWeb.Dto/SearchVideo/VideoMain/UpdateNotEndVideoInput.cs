using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 更新未完结影片数据 Input
    /// </summary>
    public class UpdateNotEndVideoInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mainId">影片Id</param>
        /// <param name="videoContentFeature">页面特征码</param>
        /// <param name="isEnd">是否完结</param>
        /// <param name="epItems">剧集信息</param>
        public UpdateNotEndVideoInput(long mainId, string videoContentFeature, bool isEnd, List<EditEpisodeItem> epItems)
        {
            MainId = mainId;
            VideoContentFeature = videoContentFeature;
            IsEnd = isEnd;
            EpItems = epItems;
        }

        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 页面特征码
        /// </summary>
        [Required(ErrorMessage = "页面特征码必填")]
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 影片Id
        /// </summary>
        [Required(ErrorMessage = "影片Id必填")]
        public long MainId { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 影片来源url
        /// </remarks>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 剧集信息
        /// </summary>
        public List<EditEpisodeItem> EpItems { get; set; }
    }
}
