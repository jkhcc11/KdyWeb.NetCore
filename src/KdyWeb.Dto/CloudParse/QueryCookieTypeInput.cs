using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询Cookie类型列表
    /// </summary>
    public class QueryCookieTypeInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(CloudParseCookieType.ShowText), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
