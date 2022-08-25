namespace KdyWeb.Dto.HttpApi
{
    /// <summary>
    /// 获取用户绑定角色信息 input
    /// </summary>
    public class QueryUserBindInfoByCookieInput
    {
        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
