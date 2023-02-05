using System;

namespace KdyWeb.Dto.HttpApi.Bilibili
{
    /// <summary>
    /// 根据视频详情页获取视频信息
    /// </summary>
    public class GetVideoInfoByDetailUrlResponse
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Pic { get; set; }

        public DateTime UpTime { get; set; }

        /// <summary>
        /// AId
        /// </summary>
        public string AId { get; set; }

        /// <summary>
        /// BId
        /// </summary>
        public string BId { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public GetVideoInfoByDetailUrlResponseCount CountInfo { get; set; }

        /// <summary>
        ///Up 信息
        /// </summary>
        public UpDataItem UpDataItem { get; set; }
    }

    public class GetVideoInfoByDetailUrlResponseCount
    {
        /// <summary>
        /// 访问量
        /// </summary>
        public int View { get; set; }

        /// <summary>
        /// 投币数量
        /// </summary>
        public int Coin { get; set; }

        /// <summary>
        /// 收藏数量
        /// </summary>
        public int Favorite { get; set; }

        /// <summary>
        /// 分享数量   
        /// </summary>
        public int Share { get; set; }

        /// <summary>
        /// 点赞
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 充电数量  elecFullInfo.total_count
        /// </summary>
        public int ElecCount { get; set; }
    }

    public class UpDataItem
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FansCount { get; set; }

        /// <summary>
        /// 关注数量
        /// </summary>
        public int FriendCount { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Vip过期时间
        /// </summary>
        public DateTime VipDueDate { get; set; }
    }
}
