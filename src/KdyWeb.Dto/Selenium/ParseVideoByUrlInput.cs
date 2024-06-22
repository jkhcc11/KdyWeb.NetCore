using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.Selenium
{
    /// <summary>
    /// 通用视频解析
    /// </summary>
    public class ParseVideoByUrlInput
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        [EnumDataType(typeof(PageLoadType))]
        public PageLoadType PageLoadType { get; set; } = PageLoadType.Normal;

        /// <summary>
        /// 是否延迟
        /// </summary>
        public bool IsDelay { get; set; }
    }
}
