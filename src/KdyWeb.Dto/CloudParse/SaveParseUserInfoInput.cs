namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 保存解析用户信息
    /// </summary>
    public class SaveParseUserInfoInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// Qq号
        /// </summary>
        public string UserQq { get; set; }

        /// <summary>
        /// 自有Api地址
        /// </summary>
        public string SelfApiUrl { get; set; }

        /// <summary>
        /// 防盗链
        /// </summary>
        public bool IsHoldLink { get; set; }

        /// <summary>
        /// 防盗链Host
        /// </summary>
        public string[] HoldLinkHost { get; set; }
    }
}
