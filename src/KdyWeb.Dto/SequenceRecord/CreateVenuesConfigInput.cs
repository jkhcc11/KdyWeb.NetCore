using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 创建场馆配置
    /// </summary>
    public class CreateVenuesConfigInput
    {
        /// <summary>
        /// 场馆名称
        /// </summary>
        [StringLength(VenuesConfig.VenuesNameLength)]
        [Required]
        public string VenuesName { get; set; }

        /// <summary>
        /// 缩写
        /// </summary>
        [StringLength(VenuesConfig.ShortNameLength)]
        [Required]
        public string ShortName { get; set; }

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
    }
}
