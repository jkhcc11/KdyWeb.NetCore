using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询Cookie类型列表
    /// </summary>
    [AutoMap(typeof(CloudParseCookieType))]
    public class QueryCookieTypeDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 显示文案
        /// </summary>
        public string ShowText { get; set; }

        /// <summary>
        /// 业务标识
        /// </summary>
        public string BusinessFlag { get; set; }
    }
}
