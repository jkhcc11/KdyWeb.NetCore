namespace KdyWeb.Dto.Selenium
{
    public class QrLoginInitWithAliOut
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 二维码Url
        /// </summary>
        public string CodeContent { get; set; }

        /// <summary>
        /// Ck
        /// </summary>
        public string CKey { get; set; }
    }
}
