using Newtonsoft.Json;

namespace KdyWeb.CloudParse.Input
{
    /// <summary>
    /// 下载基类输入
    /// </summary>
    public class BaseDownInput<TExtEntity>
    {
        /// <summary>
        /// 下载基类输入
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="fileId">FileId</param>
        /// <param name="downUrlSearchType">下载Url搜索类型</param>
        public BaseDownInput(string cacheKey, string fileId, DownUrlSearchType downUrlSearchType)
        {
            CacheKey = cacheKey;
            FileId = fileId;
            DownUrlSearchType = downUrlSearchType;
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
        /// 下载Url搜索类型
        /// </summary>
        public DownUrlSearchType DownUrlSearchType { get; set; }

        /// <summary>
        /// 是否切片
        /// </summary>
        /// <remarks>
        /// 不可能所有都支持，根据实际情况来
        /// </remarks>
        public bool IsTs { get; set; }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }

    /// <summary>
    /// 下载Url搜索类型
    /// </summary>
    public enum DownUrlSearchType
    {
        /// <summary>
        /// 名称
        /// </summary>
        Name = 1,

        /// <summary>
        /// 文件Id
        /// </summary>
        FileId = 2
    }
}
