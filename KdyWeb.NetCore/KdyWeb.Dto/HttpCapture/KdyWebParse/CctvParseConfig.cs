using KdyWeb.WebParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// CCTV解析配置
    /// </summary>
    public class CctvParseConfig : IKdyWebParseConfig
    {
        /// <summary>
        /// VN固定
        /// </summary>
        public string Vn { get; set; }

        /// <summary>
        /// 静态值
        /// </summary>
        public string StaticCheck { get; set; }

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; }

        public string ApiHost { get; set; }
    }
}
