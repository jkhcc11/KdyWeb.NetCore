using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询同演员影片列表 Input
    /// </summary>
    public class QuerySameVideoByActorInput
    {
        /// <summary>
        /// 演员名
        /// </summary>
        [Required(ErrorMessage = "演员名必填")]
        public string Actor { get; set; }
    }
}
