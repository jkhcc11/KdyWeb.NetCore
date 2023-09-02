using System.Collections.Generic;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 批量修改文件名
    /// </summary>
    public class BaseBatchUpdateNameInput
    {
        /// <summary>
        ///  子账号Id
        /// </summary>
        public long SubInfo { get; set; }

        /// <summary>
        /// 扩展Id
        /// </summary>
        /// <remarks>
        /// 扩展Id 家庭Id|企业Id|等附加
        /// </remarks>
        public string ExtId { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<BatchUpdateNameItem> FileItems { get; set; }
    }

    /// <summary>
    /// 修改文件名Item
    /// </summary>
    public class BatchUpdateNameItem
    {
        /// <summary>
        /// 旧文件名
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; set; }
    }
}
