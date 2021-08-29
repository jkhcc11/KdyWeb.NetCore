using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 变更反馈信息状态 入参
    /// </summary>
    public class ChangeFeedBackInfoInput
    {
        /// <summary>
        /// 主键集合
        /// </summary>
        [Required]
        public int[] Ids { get; set; }

        /// <summary>
        /// 变更状态
        /// </summary>
        [Required]
        public FeedBackInfoStatus FeedBackInfoStatus { get; set; }
    }
}
