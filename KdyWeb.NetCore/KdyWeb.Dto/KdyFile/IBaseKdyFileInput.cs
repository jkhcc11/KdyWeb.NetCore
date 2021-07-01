namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// 文件上传基础Input  接口
    /// </summary>
    public interface IBaseKdyFileInput
    {
        /// <summary>
        /// 文件名
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// 文件Url
        /// </summary>
        string FileUrl { get; }

        /// <summary>
        /// 文件byte
        /// </summary>
        byte[] FileBytes { get; }

        /// <summary>
        /// 设置上传文件byte数据
        /// </summary>
        void SetFileBytes(byte[] fileBytes);
    }
}
