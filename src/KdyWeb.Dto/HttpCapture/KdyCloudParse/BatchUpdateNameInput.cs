namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 批量改名
    /// </summary>
    public class BatchUpdateNameInput
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 旧文件名
        /// </summary>
        /// <remarks>
        ///  名称解析清空缓存
        /// </remarks>
        public string OldName { get; set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public string NewName { get; set; }

        public override string ToString()
        {
            return $"FileId:{FileId},OldName:{OldName},NewName:{NewName}";
        }
    }
}
