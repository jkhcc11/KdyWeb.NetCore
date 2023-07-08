namespace KdyWeb.Dto.HttpApi.Bilibili
{
    /// <summary>
    /// 根据视频详情页获取视频信息
    /// </summary>
    public class GetVideoInfoByDetailUrlRequest
    {
        /// <summary>
        /// 详情Url
        /// </summary>
        public string DetailUrl { get; set; }
    }
}
