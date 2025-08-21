using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页获取用户接龙列表
    /// </summary>
    [AutoMap(typeof(SequenceUserRecord))]
    public class QueryPageSequenceUserRecordDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        /// <remarks>
        ///  对应微信昵称
        /// </remarks>
        public string UserNickName { get; set; }

        /// <summary>
        /// 用户显示昵称
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        public string UserShowName { get; set; }
    }
}
