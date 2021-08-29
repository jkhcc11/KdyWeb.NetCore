namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// MinIo存储 输入
    /// </summary>
    public class MinIoFileInput : BaseKdyFileInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="bucketName">存储桶</param>
        /// <param name="fileName">文件名</param>
        /// <param name="location">位置</param>
        public MinIoFileInput(string bucketName, string fileName,string location = "cn-249")
        {
            BucketName = bucketName;
            Location = location;
            SetFileName(fileName);
        }

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// 存储区域 默认cn-249
        /// </summary>
        public string Location { get; set; }
    }
}
