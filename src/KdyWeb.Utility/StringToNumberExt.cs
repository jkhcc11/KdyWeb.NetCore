using System.Text.RegularExpressions;

namespace KdyWeb.Utility
{
    /// <summary>
    /// 字符串转数字相关
    /// </summary>
    public static class StringToNumberExt
    {
        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            int.TryParse(str, out var r);
            return r;
        }

        /// <summary>
        /// 字符串转Int64
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static long ToInt64(this string str)
        {
            long.TryParse(str, out var r);
            return r;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double.TryParse(str, out var r);
            return r;
        }

        /// <summary>
        /// 获取纯数字
        /// 如：http://www.baiwanzy.com/?m=vod-detail-id-14348.html  =》14348 <br/>
        /// http://agmov.com/video/play/17506 =》17506
        /// </summary>
        /// <param name="detailUrl">详情url</param>
        /// <returns></returns>
        public static string GetNumber(this string detailUrl)
        {
            var reg = new Regex(@"[1-9]\d*", RegexOptions.RightToLeft);
            var match = reg.Match(detailUrl);
            if (match.Length > 0)
            {
                return match.Value;
            }
            return string.Empty;
        }
    }
}
