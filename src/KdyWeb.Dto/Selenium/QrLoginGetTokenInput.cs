namespace KdyWeb.Dto.Selenium
{
    public class QrLoginGetTokenInput
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long AliTime { get; set; }
        
        /// <summary>
        /// 阿里Ck
        /// </summary>
        public string AliCKey { get; set; }
    }
}
