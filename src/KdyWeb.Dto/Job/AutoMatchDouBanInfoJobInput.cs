namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 自动匹配豆瓣信息
    /// </summary>
    public class AutoMatchDouBanInfoJobInput
    {
        /// <summary>
        /// 影片名
        /// </summary>
        public string VodTitle { get; set; }

        /// <summary>
        /// 影片Id
        /// </summary>
        public long MainId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VodYear { get; set; }
    }
}
