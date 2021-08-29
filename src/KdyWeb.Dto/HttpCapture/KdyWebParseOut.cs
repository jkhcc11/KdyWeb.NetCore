using KdyWeb.WebParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 通用站点解析Out
    /// </summary>
    public class KdyWebParseOut : IKdyWebParseOut
    {
        public string ResultUrl { get; set; }

        public WebParseType Type { get; set; }
    }
}
