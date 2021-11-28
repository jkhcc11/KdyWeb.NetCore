namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 解析用户登录
    /// </summary>
    public class LoginWithParseUserInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }
}
