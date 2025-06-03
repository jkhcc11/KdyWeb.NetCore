using System.Collections.Generic;
using System.ComponentModel;

namespace KdyWeb.BaseInterface.KdyOptions
{
    /// <summary>
    /// 腾讯记录配置
    /// </summary>
    public class TxDocRecordsOption
    {
        /// <summary>
        /// 用户登录Cookie
        /// </summary>
        public string UserLoginCookie { get; set; } = "uid=144115217693921056;uid_key=EOP1mMQHGixVWjhGaHM4djhlNUsyemMyU0o2T1JncnRxOC9JMmVrZm5zM2lndnNmZnI0PSKBAmV5SmhiR2NpT2lKQlEwTkJURWNpTENKMGVYQWlPaUpLVjFRaWZRLmV5SlVhVzU1U1VRaU9pSXhORFF4TVRVeU1UYzJPVE01TWpFd05UWWlMQ0pXWlhJaU9pSXhJaXdpUkc5dFlXbHVJam9pYzJGaGMxOTBiMk1pTENKU1ppSTZJblpCYjNaMFNpSXNJbVY0Y0NJNk1UYzFNRGswT0RBMk1pd2lhV0YwSWpveE56UTRNelUyTURZeUxDSnBjM01pT2lKVVpXNWpaVzUwSUVSdlkzTWlmUS5OTWJ6OFBEYVFsRVZrX25tVFVtZ2ZCbElORU9sZG1FYk45SEhSa1hDMjJ3KN6x9cIG;";

        /// <summary>
        /// 奖励用户列表
        /// </summary>
        public List<GiftUserItem> GiftUserItems { get; set; } = new();

        /// <summary>
        /// 最高价格
        /// </summary>
        /// <remarks>
        /// 不到阶梯数量 就是最高价格
        /// </remarks>
        public decimal MaxPrice { get; set; } = 25;

        /// <summary>
        /// 最低价格
        /// </summary>
        /// <remarks>
        /// 到了阶梯数量 就是最低价格
        /// </remarks>
        public decimal MinPrice { get; set; } = 20;

        /// <summary>
        /// 阶梯数量
        /// </summary>
        public int NumberSteps { get; set; } = 5;

        /// <summary>
        /// Vip用户免费阶梯
        /// </summary>
        /// <remarks>
        ///  15免1 30免2 类似
        /// </remarks>
        public int VipFreeSteps = 15;
    }

    /// <summary>
    /// 奖励用户类型
    /// </summary>
    public enum GiftUserTypeEnum
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        Normal = 0,

        /// <summary>
        /// 年卡扣卡
        /// </summary>
        [Description("扣卡")]
        Card = 1,

        /// <summary>
        /// 免费次数
        /// </summary>
        [Description("冠亚奖")]
        Free = 2,

        /// <summary>
        /// 时间区间价格（这段时间是一个固定价格）
        /// </summary>
        [Description("折扣奖")]
        TimePrice = 3,

        /// <summary>
        /// Vip
        /// </summary>
        /// <remarks>
        /// 15免1 人员
        /// </remarks>
        [Description("Vip免")]
        VipUser = 99
    }

    /// <summary>
    /// 奖励用户Item
    /// </summary>
    public class GiftUserItem
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";

        /// <summary>
        /// 昵称
        /// </summary>
        public string UserNickName { get; set; } = "";

        /// <summary>
        /// 类型
        /// </summary>
        public GiftUserTypeEnum GiftUserType { get; set; }

        /// <summary>
        /// 奖励价格
        /// </summary>
        public decimal GiftPrice { get; set; }
    }
}
