using System;
using KdyWeb.PageParse;
using KdyWeb.Utility;

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
            var index = resultUrl.IndexOf("?vfm", StringComparison.OrdinalIgnoreCase);
            if (index > 0)
            {
                //360影视 无用参数
                resultUrl = resultUrl.Substring(0, index);
            }

            ResultUrl = resultUrl;
            ResultName = resultName.RemoveStrExt("\r", "\n", " ").GetNumber();
        }

        public string PageMd5 { get; set; }

        public string ResultUrl { get; set; }

        public string ResultName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }
    }
}
