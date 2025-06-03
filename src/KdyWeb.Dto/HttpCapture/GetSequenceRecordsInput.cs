namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取腾讯文档接龙列表 input
    /// </summary>
    public class GetSequenceRecordsInput
    {
        /// <summary>
        /// 文档Url
        /// </summary>
        public string DocUrl { get; set; }

        /// <summary>
        /// 用户登录cookie
        /// </summary>
        public string UserLoginCookie { get; set; }
    }
}
