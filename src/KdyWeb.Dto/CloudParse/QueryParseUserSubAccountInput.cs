using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 分页查询用户子账号列表
    /// </summary>
    public class QueryParseUserSubAccountInput : BasePageInput
    {
        /// <summary>
        /// 子账号类型
        /// </summary>
        public CloudParseCookieType? SubAccountType { get; set; }
    }
}
