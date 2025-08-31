using KdyWeb.Entity.SequenceRecord.Enum;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface;
using Newtonsoft.Json;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 分页查询奖励用户配置
    /// </summary>
    public class QueryPageGiftUserConfigDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 奖励用户类型
        /// </summary>
        public GiftUserTypeEnum GiftUserType { get; set; }

        /// <summary>
        /// 用户显示名(冗余)
        /// </summary>
        /// <remarks>
        ///  对应群昵称
        /// </remarks>
        public string UserShowName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 奖励开始时间
        /// </summary>
        public string GiftStartTime { get; set; }

        /// <summary>
        /// 奖励结束时间
        /// </summary>
        public string GiftEndTime { get; set; }

        /// <summary>
        /// 奖励价格
        /// </summary>
        public decimal GiftPrice { get; set; }

        /// <summary>
        /// 奖励有效次数
        /// </summary>
        /// <remarks>
        ///  1、折扣卡金额大于0不用管次数
        ///  2、只有擂主免和折扣免才需要管这个次数
        /// </remarks>
        public int? GiftValidCount { get; set; }

        /// <summary>
        /// 奖励已用次数
        /// </summary>
        /// <remarks>
        /// 1、只有擂主免和折扣免才需要管这个次数
        /// 2、只有改了日期、时才可以重置次次数
        /// </remarks>
        public int GiftUseCount { get; set; }

        /// <summary>
        /// 场馆Id
        /// </summary>
        /// <remarks>
        /// 奖励绑定场馆使用
        /// </remarks>
        [JsonConverter(typeof(JsonConverterLong))]
        public long VenuesId { get; set; }

        /// <summary>
        /// 球馆名
        /// </summary>
        public string VenuesName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 奖励扣费排序（奖励叠加时扣费顺序）
        /// </summary>
        /// <remarks>
        /// 多个奖励同时存在时使用顺序（越大越靠前扣费）
        /// </remarks>
        public int GiftOrderBy { get; set; }
    }
}
