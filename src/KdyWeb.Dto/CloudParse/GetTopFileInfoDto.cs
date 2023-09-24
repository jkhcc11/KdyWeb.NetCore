namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 获取访问前N条文件信息
    /// </summary>
    public class GetTopFileInfoDto
    {
        /// <summary>
        /// 文件Id或名称
        /// </summary>
        public string FileIdOrFileName { get; set; }

        /// <summary>
        /// 次数
        /// </summary>
        public long Count { get; set; }
    }
}
