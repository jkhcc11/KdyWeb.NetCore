using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.HttpApi.DouBan
{
    /// <summary>
    /// 搜索提示 返回
    /// </summary>
    public class SearchSuggestResponse: IBaseImgUrl
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 豆瓣Url
        /// </summary>
        public string DouBanUrl { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 子标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 豆瓣主题Id
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }
    }
}
