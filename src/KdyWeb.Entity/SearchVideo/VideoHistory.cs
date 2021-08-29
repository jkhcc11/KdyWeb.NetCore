using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SearchVideo
{
    /// <summary>
    /// 视频播放记录
    /// </summary>
    public class VideoHistory : BaseEntity<long>
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
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="keyId">主表Key</param>
        /// <param name="epId">剧集Id</param>
        public VideoHistory(long keyId, long epId)
        {
            KeyId = keyId;
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

        /// <summary>
        /// 影片主表
        /// </summary>
        public virtual VideoMain VideoMain { get; set; }

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
        /// 海报
        /// </summary>
        [StringLength(DouBanInfo.VideoImgLength)]
        public string VodImgUrl { get; set; }
        #endregion

        /// <summary>
        /// 设置冗余数据
        /// </summary>
        public void SetVideoInfo(string epName, string vodName, string vodImgUrl)
        {
            EpName = epName;
            VodName = vodName;
            VodImgUrl = vodImgUrl;
        }
    }
}
