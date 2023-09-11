using System.ComponentModel.DataAnnotations;

namespace KdyWeb.CloudParse.SelfHost.Models
{
    /// <summary>
    /// 自用站解析
    /// </summary>
    public class SelfParseInput
    {
        /// <summary>
        /// 加密Url  DES HEx Str
        /// </summary>
        [Required(ErrorMessage = "Url必填")]
        public string EncodeUrl { get; set; }
    }
}
