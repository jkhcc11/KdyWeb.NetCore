namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// MinIO配置
    /// </summary>
    public class MinioConfig
    {
        /// <summary>
        /// MinIO Api地址
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 访问Key
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 访问Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public bool IsSSL { get; set; }
    }
}
