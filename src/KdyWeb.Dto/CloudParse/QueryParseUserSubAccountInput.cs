using KdyWeb.BaseInterface.BaseModel;

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
        public long? SubAccountTypeId { get; set; }
    }
}
