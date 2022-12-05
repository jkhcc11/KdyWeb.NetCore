namespace KdyWeb.Dto.KdyUser
{
    /// <summary>
    /// 获取登录Token
    /// </summary>
    public class GetLoginTokenDto
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Token 类型 默认Bearer
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
