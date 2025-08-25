using KdyWeb.Entity.KdyUserNew.Enum;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.KdyUserNew;

namespace KdyWeb.Dto.KdyUser
{
    /// <summary>
    /// 根据用户和类型获取未读Top消息
    /// </summary>
    [AutoMap(typeof(KdyMsgCenter))]
    public class GetNoReadTopMsgDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgTypeEnum MsgType { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessTypeEnum BusinessType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string MsgTitle { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
    }
}
