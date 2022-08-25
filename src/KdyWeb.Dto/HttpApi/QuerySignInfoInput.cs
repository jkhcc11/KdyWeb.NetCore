namespace KdyWeb.Dto.HttpApi
{
    /// <summary>
    /// 获取签到信息 Input
    /// </summary>
    public class QuerySignInfoInput : BaseGenShinInput
    {
        public QuerySignInfoInput(string uid, string salt,
            string version, string cookie) : base(uid, salt, version, cookie)
        {
        }

        /// <summary>
        /// 活动Id
        /// </summary>
        public string ActId { get; set; } = "e202009291139501";
    }
}
