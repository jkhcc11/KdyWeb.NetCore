using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询用户列表
    /// </summary>
    public class QueryParseUserInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(CloudParseUser.SelfApiUrl), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
