using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 批量匹配豆瓣信息
    /// </summary>
    public class BatchMatchDouBanInfoInput
    {
        /// <summary>
        /// 影片名
        /// </summary>
        [Required(ErrorMessage = "影片名不能为空")]
        public string VodTitle { get; set; }

        /// <summary>
        /// 影片Id
        /// </summary>
        [Required(ErrorMessage = "影片Id不能为空")]
        public long KeyId { get; set; }

        /// <summary>
        /// 影片年
        /// </summary>
        public int VodYear { get; set; }
    }
}
