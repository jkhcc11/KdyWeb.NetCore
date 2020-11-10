using System.Collections.Generic;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧版影视主 
    /// </summary>
    public class OldSearchSysMain : BaseEntity<int>
    {
        /// <summary>
        /// 视频类型 动作、犯罪
        /// </summary>
        public string VideoType { get; set; }

        /// <summary>
        /// 类型 电影、电视剧
        /// </summary>
        public string MovieType { get; set; }

        ///// <summary>
        ///// 排序
        ///// </summary>
        //public int SearchIndex { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        public string IsEnd { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string ResultImg { get; set; }

        /// <summary>
        /// 源url
        /// </summary>
        public string ResultUrl { get; set; }

        /// <summary>
        /// 特征码
        /// </summary>
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string VideoDescribe { get; set; }

        /// <summary>
        /// 豆瓣分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 豆瓣Url
        /// </summary>
        public string VideoDetail { get; set; }

        /// <summary>
        /// 演员
        /// </summary>
        public string VideoCasts { get; set; }

        /// <summary>
        /// 状态 0 正常 1禁用 2登录 3
        /// </summary>
        public int? VideoStatus { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string VideoCountries { get; set; }

        /// <summary>
        /// 是否匹配豆瓣
        /// </summary>
        public int IsMatchInfo { get; set; }

        /// <summary>
        /// 解说url
        /// </summary>
        public string NarrateUrl { get; set; }

        /// <summary>
        /// 版权Url
        /// </summary>
        public string BanVideoJumpUrl { get; set; }

        public virtual ICollection<OldSearchSysEpisode> Episodes { get; set; }
    }
}
