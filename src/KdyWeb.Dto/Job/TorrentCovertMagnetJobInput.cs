namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 种子转磁力
    /// </summary>
    public class TorrentCovertMagnetJobInput
    {
        /// <summary>
        /// 下载Id
        /// </summary>
        public long DownId { get; set; }

        /// <summary>
        /// 种子Url
        /// </summary>
        public string TorrentUrl { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Referer { get; set; }
    }
}
