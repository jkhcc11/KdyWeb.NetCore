namespace KdyWeb.Dto
{
    /// <summary>
    /// 用户名或邮箱是否存在 Input
    /// </summary>
    public class CheckUserExitInput
    {
        /// <summary>
        /// 邮箱或用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
