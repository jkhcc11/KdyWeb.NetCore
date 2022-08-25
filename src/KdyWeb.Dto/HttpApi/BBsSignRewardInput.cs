namespace KdyWeb.Dto.HttpApi
{
    /// <summary>
    /// bbs签到 Input
    /// </summary>
    public class BBsSignRewardInput : BaseGenShinInput
    {
        public BBsSignRewardInput(string uid, string salt,
            string version, string cookie) :
            base(uid, salt, version, cookie)
        {
        }

        /// <summary>
        /// 活动Id
        /// </summary>
        public string ActId { get; set; } = "e202009291139501";
    }
}