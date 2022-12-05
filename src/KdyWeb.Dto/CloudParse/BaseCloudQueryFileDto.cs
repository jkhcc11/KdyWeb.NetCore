using KdyWeb.CloudParse.Out;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 基础文件查询 Dto
    /// </summary>
    public class BaseCloudQueryFileDto: BaseResultOut
    {
        /// <summary>
        /// 普通外链
        /// </summary>
        public string ShowLinkUrl { get; set; }
       
        /// <summary>
        /// 强制切片
        /// </summary>
        /// <remarks>
        ///  目前仅适用于阿里云
        /// </remarks>
        public string ShowLinkUrlTs { get; set; }
    }
}
