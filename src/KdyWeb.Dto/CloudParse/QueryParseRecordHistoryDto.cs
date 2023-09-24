using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询记录
    /// </summary>
    [AutoMap(typeof(ParseRecordHistory))]
    public class QueryParseRecordHistoryDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public long SubAccountId { get; set; }

        /// <summary>
        /// 记录历史类型
        /// </summary>
        public RecordHistoryType RecordHistoryType { get; set; }

        /// <summary>
        /// 文件名或文件Id
        /// </summary>
        public string FileIdOrFileName { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
    }
}
