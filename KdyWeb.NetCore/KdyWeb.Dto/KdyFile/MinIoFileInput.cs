namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// MinIo存储 输入
    /// </summary>
    public class MinIoFileInput : IBaseKdyFileInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="bucketName">存储桶</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileUrl">文件Url</param>
        /// <param name="location">位置</param>
        /// <param name="domainUrl">访问Url 可能展示和上传不是一起</param>
        public MinIoFileInput(string bucketName, string fileName, string fileUrl, string location = "cn-249", string domainUrl = "https://img.zsxcb.net/")
        {
            BucketName = bucketName;
            Location = location;
            DomainUrl = domainUrl;
            FileName = fileName;
            FileUrl = fileUrl;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="bucketName">存储桶</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileBytes">文件字节数组</param>
        /// <param name="location">位置</param>
        /// <param name="domainUrl">访问Url 可能展示和上传不是一起</param>
        public MinIoFileInput(string bucketName, string fileName, byte[] fileBytes, string location = "cn-249", string domainUrl = "https://img.zsxcb.net/")
        {
            BucketName = bucketName;
            Location = location;
            DomainUrl = domainUrl;
            FileName = fileName;
            FileBytes = fileBytes;
        }

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// 存储区域 默认cn-249
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 访问域名 默认：https://img.zsxcb.net/
        /// </summary>
        public string DomainUrl { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// 字节
        /// </summary>
        public byte[] FileBytes { get; set; }
    }
}
