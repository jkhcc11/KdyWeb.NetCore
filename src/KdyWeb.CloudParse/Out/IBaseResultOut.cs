using KdyWeb.CloudParse.CloudParseEnum;

namespace KdyWeb.CloudParse.Out
{
    /// <summary>
    /// 基础输出
    /// </summary>
    public interface IBaseResultOut
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        CloudFileType FileType { get; set; }

        /// <summary>
        /// 文件/文件夹 编码
        /// </summary>
        string ResultId { get; set; }

        /// <summary>
        /// 文件/文件夹 名
        /// </summary>
        string ResultName { get; set; }
    }
}
