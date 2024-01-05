namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 绑定影片豆瓣信息
    /// </summary>
    public class BindVodDouBanInfoJobInput
    {
        /// <summary>
        /// 影片Id
        /// </summary>
        public long MainId { get; set; }

        /// <summary>
        /// 豆瓣Id
        /// </summary>
        public int DouBanId { get; set; }
    }
}
