namespace KdyWeb.Dto.Cache
{
    /// <summary>
    /// 跨域Token缓存
    /// </summary>
    public class CrossTokenCacheItem
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Token 类型 默认Bearer
        /// </summary>
        public string TokenType { get; set; }
    }
}
