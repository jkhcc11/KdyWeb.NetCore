using System.Security.Principal;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧版用户记录
    /// </summary>
    public class OldUserHistory : BaseEntity<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public int KeyId { get; set; }

        /// <summary>
        /// 剧集Id
        /// </summary>
        public int EpId { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        public string EpName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        public string VodName { get; set; }

        /// <summary>
        /// 播放Url
        /// </summary>
        public string VodUrl { get; set; }

    }
}
