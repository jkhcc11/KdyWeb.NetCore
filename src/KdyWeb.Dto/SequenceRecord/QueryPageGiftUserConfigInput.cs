using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页查询奖励用户配置
    /// </summary>
    public class QueryPageGiftUserConfigInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
