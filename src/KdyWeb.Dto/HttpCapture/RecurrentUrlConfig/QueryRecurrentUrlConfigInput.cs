using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 查询循环Job
    /// </summary>
    public class QueryRecurrentUrlConfigInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(RecurrentUrlConfig.RequestUrl), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
