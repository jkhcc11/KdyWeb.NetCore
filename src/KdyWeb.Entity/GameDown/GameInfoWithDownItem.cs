namespace KdyWeb.Entity.GameDown
{
    /// <summary>
    /// 下载列表Item
    /// </summary>
    public class GameInfoWithDownItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? DownName { get; set; }
        
        /// <summary>
        /// 下载地址
        /// </summary>
        public string? DownUrl { get; set; }
        
        /// <summary>
        /// 转换后的磁力
        /// </summary>
        public string? Magnet { get; set; }
    }
}
