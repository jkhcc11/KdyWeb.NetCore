using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取页面解析结果Out
    /// </summary>
    public class KdyWebPagePageOut : IKdyWebPagePageOut
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="pageMd5">特征码</param>
        /// <param name="resultUrl">结果Url</param>
        /// <param name="resultName">结果名</param>
        public KdyWebPagePageOut(string pageMd5, string resultUrl, string resultName)
        {
            PageMd5 = pageMd5;
            ResultUrl = resultUrl;
            ResultName = resultName;
        }

        public string PageMd5 { get; set; }

        public string ResultUrl { get; set; }

        public string ResultName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
