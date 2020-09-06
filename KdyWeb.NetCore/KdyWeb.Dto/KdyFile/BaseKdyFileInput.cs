namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// 文件上传基础Input 
    /// </summary>
    public class BaseKdyFileInput : IBaseKdyFileInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url">图片Url</param>
        /// <param name="fileName">文件名</param>
        public BaseKdyFileInput(string url, string fileName = null)
        {
            FileUrl = url;
            FileName = fileName;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="fileBytes">图片字节</param>
        /// <param name="fileName">文件名</param>
        public BaseKdyFileInput(byte[] fileBytes, string fileName = null)
        {
            FileBytes = fileBytes;
            FileName = fileName;
        }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public byte[] FileBytes { get; set; }
    }
}
