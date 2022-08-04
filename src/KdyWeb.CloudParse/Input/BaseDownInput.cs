using Newtonsoft.Json;

namespace KdyWeb.CloudParse.Input
{
    /// <summary>
    /// 下载基类输入
    /// </summary>
    public class BaseDownInput<TExtEntity>
    {
        /// <summary>
        /// 构造（未编码FileId）
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="fileId">FileId</param>
        public BaseDownInput(string cacheKey, string fileId)
        {
            CacheKey = cacheKey;
            FileId = fileId;
        }

        /// <summary>
        /// 构造（编码FileId）
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="fileId">FileId</param>
        /// <param name="isHex"></param>
        public BaseDownInput(string cacheKey, string fileId, bool isHex)
        {
            CacheKey = cacheKey;
            FileId = fileId;
            IsHex = true;
        }

        /// <summary>
        /// 缓存Key
        /// </summary>
        public string CacheKey { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public TExtEntity ExtData { get; set; }

        /// <summary>
        /// 是否为16进制转码
        /// </summary>
        public bool IsHex { get; set; }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
