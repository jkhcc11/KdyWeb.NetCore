namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 定时影片录入Job Input
    /// </summary>
    public class RecurringVideoJobInput
    {
        /// <summary>
        /// 首页地址
        /// </summary>
        public string BaseHost { get; set; }

        /// <summary>
        /// 源地址
        /// </summary>
        public string OriginUrl { get; set; }

        /// <summary>
        /// 采集详情匹配Xpath
        /// </summary>
        public string CaptureDetailXpath { get; set; }

        /// <summary>
        /// 采集详情名称处理
        /// </summary>
        /// <remarks>
        ///  xxxxBD高清->xxxx <br/>
        ///  xxxxHD高清->xxxx <br/>
        ///  xxxx更新至36集->xxxx <br/>
        /// </remarks>
        public string[] CaptureDetailNameSplit { get; set; }
    }
}
