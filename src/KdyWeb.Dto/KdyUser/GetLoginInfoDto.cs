namespace KdyWeb.Dto.KdyUser
{
    /// <summary>
    /// 获取用户登录信息
    /// </summary>
    public class GetLoginInfoDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
    }
}
