using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 自动匹配并保存剧集
    /// </summary>
    public class AutoMatchSaveEpInput
    {
        /// <summary>
        /// 豆瓣DbId
        /// </summary>
        [Range(100, int.MaxValue, ErrorMessage = "豆瓣DbId错误")]
        public int DouBanInfoId { get; set; }

        /// <summary>
        /// 剧集信息
        /// </summary>
        public List<EditEpisodeItem> EpItems { get; set; }

        /// <summary>
        /// 资源md5-自助提交有
        /// </summary>
        public string ZyPageMd5 { get; set; }

        /// <summary>
        /// 资源详情-自助提交有
        /// </summary>
        public string ZyDetailUrl { get; set; }
    }
}
