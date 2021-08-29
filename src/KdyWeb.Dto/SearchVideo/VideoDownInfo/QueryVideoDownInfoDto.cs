using System.Collections.Generic;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;
using Newtonsoft.Json;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 查询下载地址 Dto
    /// </summary>
    public class QueryVideoDownInfoDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 影片名
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 特征码
        /// </summary>
        public string UrlFeature { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public IList<DownUrlJsonItem> Items { get; set; }
    }

    public class QueryVideoDownInfoDtoProfile : Profile
    {
        public QueryVideoDownInfoDtoProfile()
        {
            CreateMap<VideoDownInfo, QueryVideoDownInfoDto>()
                .ForMember(a => a.Items,
                    a => a.MapFrom(b => JsonConvert.DeserializeObject<IList<DownUrlJsonItem>>(b.DownJson)));
        }
    }

}
