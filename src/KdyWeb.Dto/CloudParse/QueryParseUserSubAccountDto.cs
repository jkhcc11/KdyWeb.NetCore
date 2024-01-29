using System;
using System.Linq;
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

        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        /// 有些下载需要固定带上附加ID，如 分组ID等，这种不支持跨云盘切换，所以手动指定
        /// </remarks>
        public string BusinessId { get; set; }

        /// <summary>
        /// 关联用户Ids 多个逗号隔开,
        /// </summary>
        /// <remarks>
        /// 多个用户共用子账号信息
        /// </remarks>
        public string RelationalUserIds { get; set; }

        public string[] RelationalUserArray =>
            string.IsNullOrEmpty(RelationalUserIds)
                ? null
                : RelationalUserIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();
    }
}
