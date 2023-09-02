using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 基础文件查询Input
    /// </summary>
    public class BaseCloudQueryFileInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        ///  子账号Id
        /// </summary>
        public long SubAccountId { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 扩展Id
        /// </summary>
        /// <remarks>
        /// 扩展Id 家庭Id|企业Id|等附加
        /// </remarks>
        public string ExtId { get; set; }
    }
}
