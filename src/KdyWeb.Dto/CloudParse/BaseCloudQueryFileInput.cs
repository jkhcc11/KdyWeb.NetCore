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
        public long SubInfo { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; set; }
    }
}
