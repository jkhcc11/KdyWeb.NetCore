namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 基于Server酱发送消息job inputs
    /// </summary>
    public class SendMsgWithFtqqJobInput
    {
        /// <summary>
        /// 给谁发 @人
        /// </summary>
        public string ToUser { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
