namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 根据影片源详情创建影片 Input
    /// </summary>
    public class CreateVideoInfoByDetailInput
    {
        /// <summary>
        /// 详情Url
        /// </summary>
        public string DetailUrl { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        public string VideoName { get; set; }
    }
}
