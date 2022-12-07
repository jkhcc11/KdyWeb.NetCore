namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 游戏下载解析
    /// </summary>
    public class GameDownCaptureJobInput
    {
        /// <summary>
        /// 详情
        /// </summary>
        public string DetailUrl { get; set; }

        /// <summary>
        /// 是否翻页Url
        /// </summary>
        public bool IsPageUrl { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get; set; }
    }
}
