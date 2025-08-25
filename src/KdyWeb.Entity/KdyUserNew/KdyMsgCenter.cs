using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.KdyUserNew.Enum;

namespace KdyWeb.Entity.KdyUserNew
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public class KdyMsgCenter : BaseEntity<long>
    {
        public const int MsgTitleLength = 50;
        public const int MsgContentLength = 500;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="msgTitle">消息标题</param>
        /// <param name="msgContent">消息内容</param>
        public KdyMsgCenter(MsgTypeEnum msgType, BusinessTypeEnum businessType, string msgTitle, string msgContent)
        {
            MsgType = msgType;
            MsgTitle = msgTitle;
            MsgContent = msgContent;
            BusinessType = businessType;
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgTypeEnum MsgType { get; protected set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessTypeEnum BusinessType { get; protected set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        [StringLength(MsgTitleLength)]
        public string MsgTitle { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [StringLength(MsgContentLength)]
        public string MsgContent { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; protected set; }


        /// <summary>
        /// 用户Id
        /// </summary>
        /// <remarks>
        /// 非系统通知时
        /// </remarks>
        public long? UserId { get; set; }

        /// <summary>
        /// 设置为已读
        /// </summary>
        public void Read()
        {
            IsRead = true;
        }
    }
}
