namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 通用解析
    /// </summary>
    public class CommonParseDto
    {
        /// <summary>
        /// 通用解析
        /// </summary>
        /// <param name="downLink">下载地址</param>
        public CommonParseDto(string downLink)
        {
            DownLink = downLink;
        }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownLink { get; set; }
    }
}
