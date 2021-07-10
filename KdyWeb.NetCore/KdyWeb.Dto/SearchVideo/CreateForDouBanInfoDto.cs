using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 通过豆瓣信息创建影片信息 Dto
    /// </summary>
    [AutoMap(typeof(VideoMain))]
    public class CreateForDouBanInfoDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 视频名称
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 是否匹配影片信息Url
        /// </summary>
        public bool IsMatchInfo { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

    }
}
