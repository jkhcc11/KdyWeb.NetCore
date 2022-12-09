namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 获取Steam信息Job
    /// </summary>
    public class SteamInfoJobInput
    {
        /// <summary>
        /// 下载ID
        /// </summary>
        public long DownId { get; set; }
        
        /// <summary>
        /// 是否直接Url
        /// </summary>
        public bool IsSteamUrl { get; set; }
        
        /// <summary>
        /// StreamUrl
        /// </summary>
        public string SteamUrl { get; set; }
        
        /// <summary>
        /// 自定义Id
        /// </summary>
        public string CustomId { get; set; }
        
        /// <summary>
        /// 用户Hash
        /// </summary>
        public string UserHash { get; set; }
    }
}
