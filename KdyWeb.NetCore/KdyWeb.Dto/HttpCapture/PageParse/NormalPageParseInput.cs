using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 通用站点页面解析 Input
    /// </summary>
    public class NormalPageParseInput : IPageParseInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 详情Url
        /// </summary>
        public string Detail { get; set; }

        public long ConfigId { get; set; }
    }
}
