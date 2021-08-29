namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// 文件上传返回Dto
    /// </summary>
    public class KdyFileDto
    {
        /// <summary>
        /// 上传后的Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件Md5
        /// </summary>
        public string FileMd5 { get; set; }
    }
}
