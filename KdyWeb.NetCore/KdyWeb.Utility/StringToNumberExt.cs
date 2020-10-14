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
        /// 字符串转double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double.TryParse(str, out var r);
            return r;
        }
    }
}
