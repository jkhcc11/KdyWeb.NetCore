namespace Kdy.StandardJob.JobInput
{
    /// <summary>
    /// 豆瓣信息录入Job 入参
    /// </summary>
    public class SaveDouBanInfoInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="subjectId">豆瓣Id</param>
        /// <param name="userEmail">用户邮箱</param>
        public SaveDouBanInfoInput(string subjectId, string userEmail)
        {
            SubjectId = subjectId;
            UserEmail = userEmail;
        }

        /// <summary>
        ///  豆瓣Id
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 用户Url
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
