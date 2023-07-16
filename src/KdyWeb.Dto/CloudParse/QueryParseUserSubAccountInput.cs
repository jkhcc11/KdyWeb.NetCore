using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 分页查询用户子账号列表
    /// </summary>
    public class QueryParseUserSubAccountInput : BasePageInput
    {
        /// <summary>
        /// 子账号类型Id
        /// </summary>
        [KdyQuery(nameof(CloudParseUserChildren.CloudParseCookieTypeId), KdyOperator.Equal)]
        public long? SubAccountTypeId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(CloudParseUserChildren.Alias), KdyOperator.Like)]
        [KdyQuery(nameof(CloudParseUserChildren.OldSubAccountInfo), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
