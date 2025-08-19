using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页获取用户接龙列表
    /// </summary>
    public class QueryPageSequenceUserRecordInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
