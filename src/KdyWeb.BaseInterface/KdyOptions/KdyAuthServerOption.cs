namespace KdyWeb.BaseInterface.KdyOptions
{
    /// <summary>
    /// 授权服务器 配置
    /// </summary>
    public class KdyAuthServerOption
    {
        /// <summary>
        /// 授权服务器Host
        /// </summary>
        public string AuthHost { get; set; }

        /// <summary>
        /// 是否Https
        /// </summary>
        public bool IsRequireHttps { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 授权所有api范围
        /// </summary>
        public string AllScope { get; set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 授权管理Api
        /// </summary>
        public string AuthMgrApiHost { get; set; }

        /// <summary>
        /// 授权管理客户端密钥
        /// </summary>
        public string AuthMgrSecret { get; set; }

        /// <summary>
        ///  授权管理用户名
        /// </summary>
        public string AuthMgrUser { get; set; }

        /// <summary>
        /// 授权管理密码
        /// </summary>
        public string AuthMgrUserPwd { get; set; }
    }
}
