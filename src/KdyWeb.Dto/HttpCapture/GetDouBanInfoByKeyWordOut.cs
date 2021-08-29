namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 根据关键字获取豆瓣搜索结果 Out
    /// </summary>
    public class GetDouBanInfoByKeyWordOut
    {
        /// <summary>
        /// 结果
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// 豆瓣详情id
        /// </summary>
        public string DouBanSubjectId { get; set; }
    }
}
