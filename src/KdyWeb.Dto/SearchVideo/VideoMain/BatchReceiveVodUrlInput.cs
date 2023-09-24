using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 批量接收播放地址入库
    /// </summary>
    public class BatchReceiveVodUrlInput
    {
        /// <summary>
        /// 影片名
        /// </summary>
        [Required(ErrorMessage = "影片名必填")]
        public string VodName { get; set; }

        /// <summary>
        /// 播放地址
        /// </summary>
        /// <remarks>
        ///  名称$xxxxxx 换行
        ///  名称1$xxxxxx 换行
        /// </remarks>
        [Required(ErrorMessage = "播放地址必填")]
        public string VodPlayUrls { get; set; }

        /// <summary>
        /// 入库密码
        /// </summary>
        [Required(ErrorMessage = "入库密码必填")]
        public string Token { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [Range(1000, 3000)]
        public int Year { get; set; }
    }
}
