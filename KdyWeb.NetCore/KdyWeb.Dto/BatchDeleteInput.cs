using System.Collections.Generic;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 批量删除Input
    /// </summary>
    public class BatchDeleteInput<TKey>
    {
        /// <summary>
        /// 主键集合
        /// </summary>
        public List<TKey> Ids { get; set; }
    }

    /// <summary>
    /// 批量删除Long主键
    /// </summary>
    public class BatchDeleteForLongKeyInput : BatchDeleteInput<long>
    {

    }

    /// <summary>
    /// 批量删除Int主键
    /// </summary>
    public class BatchDeleteForIntKeyInput : BatchDeleteInput<int>
    {

    }
}
