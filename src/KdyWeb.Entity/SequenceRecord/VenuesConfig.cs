using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SequenceRecord
{
    /// <summary>
    /// 场馆配置
    /// </summary>
    public class VenuesConfig : BaseEntity<long>
    {
        public const int VenuesNameLength = 20;
        public const int ShortNameLength = 10;

        /// <summary>
        /// 场馆配置
        /// </summary>
        /// <param name="venuesName">场馆名称</param>
        /// <param name="shortName">缩写</param>
        public VenuesConfig(string venuesName, string shortName)
        {
            VenuesName = venuesName;
            ShortName = shortName;
            IsEnableGift = true;
        }

        /// <summary>
        /// 场馆名称
        /// </summary>
        [StringLength(VenuesNameLength)]
        public string VenuesName { get; protected set; }

        /// <summary>
        /// 是否启用奖励计算
        /// </summary>
        public bool IsEnableGift { get; protected set; }

        /// <summary>
        /// 缩写
        /// </summary>
        [StringLength(ShortNameLength)]
        public string ShortName { get; protected set; }

        /// <summary>
        /// 最高价格
        /// </summary>
        /// <remarks>
        /// 不到阶梯数量 就是最高价格
        /// </remarks>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// 最低价格
        /// </summary>
        /// <remarks>
        /// 到了阶梯数量 就是最低价格
        /// </remarks>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// 阶梯数量
        /// </summary>
        public int NumberSteps { get; set; }

        /// <summary>
        /// Vip用户免费阶梯
        /// </summary>
        /// <remarks>
        ///  15免1 30免2 类似
        /// </remarks>
        public int VipFreeSteps { get; set; }

        /// <summary>
        /// 禁用奖励计算
        /// </summary>
        public void Ban()
        {
            IsEnableGift = false;
        }
    }
}
