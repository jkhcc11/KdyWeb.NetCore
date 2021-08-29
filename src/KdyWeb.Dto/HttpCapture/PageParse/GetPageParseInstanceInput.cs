
namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取页面搜索实例 Input
    /// </summary>
    public class GetPageParseInstanceInput
    {
        /// <summary>
        /// 搜索配置Id
        /// </summary>
        public long? ConfigId { get; set; }
        
        /// <summary>
        /// 站点域名
        /// </summary>
        public string BaseHost { get; set; }
    }
}
