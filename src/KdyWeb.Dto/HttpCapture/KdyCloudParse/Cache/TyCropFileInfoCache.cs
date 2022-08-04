namespace KdyWeb.Dto.HttpCapture.KdyCloudParse.Cache
{
    /// <summary>
    /// 天翼企业云文件下载临时缓存
    /// </summary>
    public class TyCropFileInfoCache
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// 临时下载地址
        /// </summary>
        public string TempDownUrl { get; set; }
    }
}
