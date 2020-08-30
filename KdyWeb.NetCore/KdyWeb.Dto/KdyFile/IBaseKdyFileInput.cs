namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// 文件上传基础 接口
    /// </summary>
    public interface IBaseKdyFileInput
    {
        /// <summary>
        /// 文件名
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// 文件Url
        /// </summary>
        string FileUrl { get; set; }

        /// <summary>
        /// 文件byte
        /// </summary>
        byte[] FileBytes { get; set; }
    }
}
