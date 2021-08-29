using Newtonsoft.Json;

namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 影片采集 Job Input
    /// </summary>
    public class VideoCaptureJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="detailUrl">详情Url</param>
        /// <param name="videoName">影片名</param>
        /// <param name="serviceFullName">处理服务名</param>
        [JsonConstructor]
        public VideoCaptureJobInput(string detailUrl, string videoName, string serviceFullName)
        {
            DetailUrl = detailUrl;
            VideoName = videoName;
            ServiceFullName = serviceFullName;
        }

        /// <summary>
        /// 详情Url
        /// </summary>
        public string DetailUrl { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 处理服务名
        /// </summary>
        public string ServiceFullName { get; set; }

        /// <summary>
        /// 处理服务名 常量
        /// </summary>
        public class ServiceFullNameConst
        {
            /// <summary>
            /// 下载站点处理服务名
            /// </summary>
            public const string DownServiceFullName = "KdyWeb.Service.HttpCapture.DownPageParseService";
        }
    }
}
