namespace KdyWeb.Dto.HttpApi
{
    /// <summary>
    /// Genshin baseInput
    /// </summary>
    public abstract class BaseGenShinInput
    {
        /// <summary>
        /// Genshin构造
        /// </summary>
        /// <param name="uid">用户Id(roleId)</param>
        /// <param name="salt">salt</param>
        /// <param name="version">版本</param>
        /// <param name="cookie">cookie</param>
        protected BaseGenShinInput(string uid, string salt,
            string version, string cookie)
        {
            Uid = uid;
            Salt = salt;
            Version = version;
            Cookie = cookie;
        }

        /// <summary>
        /// 用户Id(roleId)
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 区域（服务器 cn_gf01 国服）
        /// </summary>
        public string Region { get; set; } = "cn_gf01";

        /// <summary>
        /// Salt
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie { get; set; }
    }

    /// <summary>
    /// 查询实时便签
    /// </summary>
    public class QueryDailyNoteInput : BaseGenShinInput
    {
        public QueryDailyNoteInput(string uid, string salt,
            string version, string cookie) :
            base(uid, salt, version, cookie)
        {

        }
    }
}
