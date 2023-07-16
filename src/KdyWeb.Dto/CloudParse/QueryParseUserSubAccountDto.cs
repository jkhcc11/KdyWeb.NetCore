using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 分页查询用户子账号列表
    /// </summary>
    [AutoMap(typeof(CloudParseUserChildren))]
    public class QueryParseUserSubAccountDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 子账号Id
        /// </summary>
        [SourceMember(nameof(CloudParseUserChildren.Id))]
        [JsonConverter(typeof(JsonConverterLong))]
        public long SubAccountId { get; set; }

        /// <summary>
        ///  Cookie类型
        /// </summary>
        [SourceMember(nameof(CloudParseUserChildren.CloudParseCookieTypeId))]
        [JsonConverter(typeof(JsonConverterLong))]
        public long SubAccountTypeId { get; set; }

        public string SubAccountTypeStr { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        [SourceMember(nameof(CloudParseUserChildren.CookieInfo))]
        public string Cookie { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 旧子账号信息
        /// </summary>
        public string OldSubAccountInfo { get; set; }
    }
}
