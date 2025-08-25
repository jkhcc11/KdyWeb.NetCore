using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.KdyUserNew.Enum;

namespace KdyWeb.Dto.KdyUser
{
    /// <summary>
    /// 根据用户和类型获取未读Top消息
    /// </summary>
    public class GetNoReadTopMsgInput
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        [EnumDataType(typeof(MsgTypeEnum))]
        public MsgTypeEnum MsgType { get; set; }
    }
}
