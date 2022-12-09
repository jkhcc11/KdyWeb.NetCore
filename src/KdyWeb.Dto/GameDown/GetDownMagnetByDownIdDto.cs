namespace KdyWeb.Dto.GameDown
{
    /// <summary>
    /// 根据下载Id获取磁力下载
    /// </summary>
    public class GetDownMagnetByDownIdDto
    {
        /// <summary>
        /// 是否磁力
        /// </summary>
        public bool IsMagnet { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownUrl { get; set; }
    }
}
