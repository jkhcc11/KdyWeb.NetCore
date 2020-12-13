using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧版用户录入
    /// </summary>
    public class OldFeedBackInfo : BaseEntity<int>
    {
        /// <summary>
        /// 豆瓣链接
        /// </summary>
        [StringLength(1000)]
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <remarks>
        /// 已忽略、待审核、正常、资源录入完毕
        /// </remarks>
        [StringLength(50)]
        public string Status { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        [StringLength(100)]
        public string UserEmail { get; set; }

        /// <summary>
        /// 名称年份
        /// </summary>
        [StringLength(50)]
        public string Remark { get; set; }

        /// <summary>
        /// 豆瓣Id
        /// </summary>
        /// <remarks>
        ///  通过此Id匹配主表记录
        /// </remarks>
        [StringLength(50)]
        public string DouBanId { get; set; }
    }
}
