namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 匹配豆瓣信息 Input
    /// </summary>
    public class MatchDouBanInfoInput
    {
        /// <summary>
        /// 影片主键
        /// </summary>
        public long KeyId { get; set; }

        /// <summary>
        /// 豆瓣信息Id
        /// </summary>
        public int DouBanId { get; set; }
    }
}
