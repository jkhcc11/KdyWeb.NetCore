namespace KdyWeb.BaseInterface.HangFire
{
    /// <summary>
    /// HangFire授权配置
    /// </summary>
    public class KdyHangFireAuthOption
    {
        /// <summary>
        /// 面板路径
        /// </summary>
        public string BasePath { get; set; } = "/kdyHangFire";

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 是否强制Https
        /// </summary>
        public bool RequireSsl { get; set; } = false;

        /// <summary>
        /// 是否自动跳转Https
        /// </summary>
        public bool SslRedirect { get; set; } = false;

        /// <summary>
        /// 是否区分大小写
        /// </summary>
        public bool LoginCaseSensitive { get; set; } = true;
    }
}
