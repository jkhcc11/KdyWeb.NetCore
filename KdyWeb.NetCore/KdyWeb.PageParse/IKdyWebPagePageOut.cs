namespace KdyWeb.PageParse
{
    /// <summary>
    /// 获取页面解析结果Out 接口
    /// </summary>
    public interface IKdyWebPagePageOut
    {
        /// <summary>
        /// 结果Url
        /// </summary>
        string ResultUrl { get; set; }

        /// <summary>
        /// 结果名称
        /// </summary>
        string ResultName { get; set; }

        /// <summary>
        /// 页面特征码
        /// </summary>
        string PageMd5 { get; set; }
    }

}
