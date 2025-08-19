using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页查询场馆配置
    /// </summary>
    public class QueryPageVenuesConfigInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
