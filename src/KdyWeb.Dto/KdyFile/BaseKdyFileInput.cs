using Newtonsoft.Json;

namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// 抽象文件上传基础Input 
    /// </summary>
    public abstract class BaseKdyFileInput : IBaseKdyFileInput
    {
        public string FileName { get; private set; }

        public string FileUrl { get; private set; }

        [JsonIgnore]
        public byte[] FileBytes { get; private set; }

        /// <summary>
        /// 设置上传文件名
        /// </summary>
        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// 设置上传文件Url
        /// </summary>
        public void SetFileUrl(string fileUrl)
        {
            FileUrl = fileUrl;
        }

        /// <summary>
        /// 设置上传文件byte数据
        /// </summary>
        public void SetFileBytes(byte[] fileBytes)
        {
            FileBytes = fileBytes;
            //二选一模式 有byte后文件url为空
            FileUrl = string.Empty;
        }

        /// <summary>
        /// 重写ToString 方便Logging日志记录
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
