namespace KdyWeb.PageParse
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPageParseInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        string KeyWord { get; set; }

        /// <summary>
        /// 详情Url
        /// </summary>
        string Detail { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        long ConfigId { get; set; }
    }
}
