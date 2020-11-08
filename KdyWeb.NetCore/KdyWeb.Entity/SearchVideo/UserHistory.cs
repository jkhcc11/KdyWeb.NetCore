using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 用户播放记录
    /// </summary>
    public class UserHistory : BaseEntity<long>
    {
        #region 常量
        /// <summary>
        /// 剧集名 长度
        /// </summary>
        public const int EpNameLength = 100;
        /// <summary>
        /// 影片名 长度
        /// </summary>
        public const int VodNameLength = 100;
        /// <summary>
        /// 播放地址 长度
        /// </summary>
        public const int VodUrlLength = 300;
        /// <summary>
        /// 用户名 长度
        /// </summary>
        public const int UserNameLength = 50;
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public UserHistory(long epId)
        {
            EpId = epId;
        }

        /// <summary>
        /// 主表主键Key
        /// </summary>
        public long KeyId { get; set; }

        /// <summary>
        /// 剧集Key
        /// </summary>
        public long EpId { get; set; }

        #region 冗余

        /// <summary>
        /// 剧集名
        /// </summary>
        public string EpName { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        public string VodName { get; set; }

        /// <summary>
        /// 播放地址
        /// </summary>
        public string VodUrl { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        #endregion
    }
}
