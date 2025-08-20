using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SequenceRecord
{
    /// <summary>
    /// 根据腾讯接龙记录创建接龙记录
    /// </summary>
    public class CreateSequenceUseRecordByTxDocInput
    {
        /// <summary>
        /// 腾讯文档接龙记录
        /// </summary>
        [Required]
        public string TxDocUrl { get; set; }

        /// <summary>
        /// 是否强制同步
        /// </summary>
        public bool? IsForcedSync { get; set; }

        ///// <summary>
        ///// 统计时间（默认为当天）
        ///// </summary>
        //public DateTime? TotalDate { get; set; }
    }
}
