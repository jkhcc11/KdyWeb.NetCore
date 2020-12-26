using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取页面解析结果Input
    /// </summary>
    public class KdyWebPagePageInput : IKdyWebPagePageInput
    {
        public string DetailUrl { get; set; }
    }
}
