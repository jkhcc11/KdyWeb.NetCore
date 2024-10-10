namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 蓝奏优享用户信息缓存
    /// </summary>
    public class LanZouYxUserInfoCache
    {
        /// <summary>
        /// 设备唯一标识 应该是
        /// </summary>
        public string DevCode { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// token 应该是1天的时间
        /// </summary>
        public string AppToken { get; set; }
    }
}
