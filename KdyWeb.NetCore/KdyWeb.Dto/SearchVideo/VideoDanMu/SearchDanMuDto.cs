using AutoMapper;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;
using Newtonsoft.Json;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 弹幕搜索 Dto
    /// </summary>
    [AutoMap(typeof(VideoDanMu))]
    public class SearchDanMuDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 弹幕内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 剧集Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long EpId { get; set; }

        /// <summary>
        /// 剧集信息
        /// </summary>
        public string EpInfo { get; set; }
    }
}
