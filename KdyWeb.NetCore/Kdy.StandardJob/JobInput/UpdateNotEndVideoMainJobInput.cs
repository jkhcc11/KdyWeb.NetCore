namespace Kdy.StandardJob.JobInput
{
    /// <summary>
    /// 更新未完结视频Job Input
    /// </summary>
    public class UpdateNotEndVideoMainJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mainId">影片主表Id</param>
        /// <param name="sourceUrl">源Url</param>
        /// <param name="videoContentFeature">源Url特征码</param>
        public UpdateNotEndVideoMainJobInput(long mainId, string sourceUrl, string videoContentFeature)
        {
            MainId = mainId;
            SourceUrl = sourceUrl;
            VideoContentFeature = videoContentFeature;
        }

        /// <summary>
        /// 影片主表Id
        /// </summary>
        public long MainId { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 源Url特征码
        /// </summary>
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string KeyWord { get; set; }
    }
}
