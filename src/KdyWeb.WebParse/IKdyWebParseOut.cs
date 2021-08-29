using System.ComponentModel.DataAnnotations;

namespace KdyWeb.WebParse
{
    /// <summary>
    /// 站点解析Out 接口
    /// </summary>
    public interface IKdyWebParseOut
    {
        /// <summary>
        /// 结果Url
        /// </summary>
        string ResultUrl { get; set; }

        /// <summary>
        /// 站点解析类型
        /// </summary>
        WebParseType Type { get; set; }
    }

    /// <summary>
    /// 站点解析类型
    /// </summary>
    public enum WebParseType
    {
        /// <summary>
        /// Mp4
        /// </summary>
        [Display(Name = "MP4")]
        Mp4,

        /// <summary>
        /// m3u8
        /// </summary>
        [Display(Name = "M3u8")]
        M3u8
    }
}
