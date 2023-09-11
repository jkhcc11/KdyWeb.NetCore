using KdyWeb.CloudParse.CloudParseEnum;

namespace KdyWeb.CloudParse.Out
{
    /// <summary>
    /// 输出结果基类
    /// </summary>
    public class BaseResultOut : IBaseResultOut
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public CloudFileType FileType { get; set; }

        /// <summary>
        /// 文件/文件夹 编码
        /// </summary>
        public string ResultId { get; set; }

        /// <summary>
        /// 文件/文件夹 名
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// 父节点 类型为非文件夹时 该字段为所属文件夹Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 文件大小 字节
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件大小 （MB）
        /// </summary>
        public int FileSizeMb => (int)(FileSize / 1024 / 1024);

        /// <summary>
        /// 文件大小 （GB）
        /// </summary>
        public int FileSizeGb => FileSizeMb / 1024;

        /// <summary>
        /// 是否为根节点
        /// </summary>
        /// <remarks>
        /// 如果为根节点时 ParentId 为根，后面每个请求都需要带上ParentId
        /// </remarks>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 是否缓存映射
        /// </summary>
        /// <remarks>
        /// 文件名和文件Id映射过
        /// </remarks>
        public bool IsCacheMap { get; set; }
    }
}
