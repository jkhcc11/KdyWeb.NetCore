namespace KdyWeb.Dto.HttpApi.AuthCenter
{
    /// <summary>
    /// 创建用户 Response
    /// </summary>
    public class CreateUserResponse
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
