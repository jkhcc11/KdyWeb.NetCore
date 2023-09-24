using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 影片发送入库
    /// </summary>
    public class ParseVodSendInput
    {
        /// <summary>
        /// Cms Api地址
        /// </summary>
        [Required(ErrorMessage = "Api地址 必填")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// 影片类型名
        /// </summary>
        [Required(ErrorMessage = "影片类型名 必填")]
        public string VodTypeName { get; set; }

        /// <summary>
        /// 入库密码
        /// </summary>
        [Required(ErrorMessage = "入库密码 必填")]
        public string SendPassWord { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        [Required(ErrorMessage = "影片名 必填")]
        public string VodName { get; set; }

        /// <summary>
        /// 播放器（字符串）
        /// </summary>
        [Required(ErrorMessage = "播放器 必填")]
        public string PlayFrom { get; set; }

        /// <summary>
        /// 播放链接（带格式的字符串）
        /// xxxx$x111 <br/>
        /// xxxx$2222
        /// </summary>
        [Required(ErrorMessage = "播放链接 必填")]
        public string PlayUrl { get; set; }

        /// <summary>
        /// Cms类型
        /// </summary>
        public CmsSendType SendType { get; set; } = CmsSendType.Mac10;
    }

    /// <summary>
    /// cms类型
    /// </summary>
    public enum CmsSendType
    {
        /// <summary>
        /// 苹果V10
        /// </summary>
        Mac10 = 1,

        /// <summary>
        /// 苹果V8
        /// </summary>
        Mac8,

        /// <summary>
        /// 自有
        /// </summary>
        Self = 100
    }
}
