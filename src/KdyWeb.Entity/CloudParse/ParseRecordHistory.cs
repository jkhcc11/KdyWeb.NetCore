using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse.Enum;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 解析记录
    /// </summary>
    public class ParseRecordHistory : BaseEntity<long>
    {
        public const int FileIdOrFileNameLength = 500;

        /// <summary>
        /// 解析记录
        /// </summary>
        /// <param name="recordHistoryType">记录历史类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="subAccountId">子账号Id</param>
        public ParseRecordHistory(RecordHistoryType recordHistoryType, long userId, long subAccountId)
        {
            RecordHistoryType = recordHistoryType;
            UserId = userId;
            SubAccountId = subAccountId;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; protected set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public long SubAccountId { get; protected set; }

        /// <summary>
        /// 记录历史类型
        /// </summary>
        public RecordHistoryType RecordHistoryType { get; protected set; }

        /// <summary>
        /// 文件名或文件Id
        /// </summary>
        [StringLength(FileIdOrFileNameLength)]
        public string? FileIdOrFileName { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [StringLength(CloudParseUserChildren.AliasLength)]
        public string? Alias { get; set; }
    }
}
