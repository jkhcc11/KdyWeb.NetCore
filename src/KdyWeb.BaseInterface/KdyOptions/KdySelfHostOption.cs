namespace KdyWeb.BaseInterface.KdyOptions
{
    /// <summary>
    /// 自有Host
    /// </summary>
    public class KdySelfHostOption
    {
        /// <summary>
        /// 图床Host
        /// </summary>
        public string ImgHost { get; set; }

        /// <summary>
        /// 伪防盗代理Host
        /// </summary>
        public string ProxyHost { get; set; }
    }
}
