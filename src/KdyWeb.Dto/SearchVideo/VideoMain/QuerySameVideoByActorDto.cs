using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using Newtonsoft.Json;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询同演员影片列表Dto
    /// </summary>
    public class QuerySameVideoByActorDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 影片Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long MainId { get; set; }

        /// <summary>
        /// 影片名
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 影片海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

    }
}
