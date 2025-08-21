using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SequenceRecord.Enum;

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

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        public GiftUserTypeEnum? GiftUserType { get; set; }
    }
}
