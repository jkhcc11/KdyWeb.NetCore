using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 获取影片信息 Dto
    /// </summary>
    [AutoMap(typeof(VideoMain))]
    public class GetVideoDetailDto : CreatedUserDto<long>, IBaseImgUrl
    {
        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 影片类型Str
        /// </summary>
        public string SubtypeStr => Subtype.GetDisplayName();

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越展示靠前
        /// </remarks>
        public int OrderBy { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string VideoImg { get; set; }

        /// <summary>
        /// 是否匹配影片信息Url
        /// </summary>
        public bool IsMatchInfo { get; set; }

        /// <summary>
        /// 影片状态
        /// </summary>
        public VideoMainStatus VideoMainStatus { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        public string Aka { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 影片来源url
        /// </remarks>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 源Url特征码
        /// </summary>
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 影片信息Url
        /// </summary>
        /// <remarks>
        /// 豆瓣Url或其他影片介绍地址
        /// </remarks>
        public string VideoInfoUrl { get; set; }

        /// <summary>
        /// 影片主表 扩展信息
        /// </summary>
        public VideoMainInfoDto VideoMainInfo { get; set; }

        /// <summary>
        /// 剧集信息组
        /// </summary>
        public List<VideoEpisodeGroupDto> EpisodeGroup { get; set; }

        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool IsSubscribe { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 影片所属系列
        /// </summary>
        public QueryVideoSeriesDto VideoSeries { get; set; }

        /// <summary>
        /// 最新的用户历史记录
        /// </summary>
        public QueryUserHistoryDto NewUserHistory { get; set; }
    }

    /// <summary>
    /// 扩展信息
    /// </summary>
    [AutoMap(typeof(VideoMainInfo))]
    public class VideoMainInfoDto
    {
        /// <summary>
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string VideoGenres { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string VideoSummary { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        public string VideoCasts { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string VideoCountries { get; set; }

        /// <summary>
        /// 解说Url
        /// </summary>
        public string NarrateUrl { get; set; }

        /// <summary>
        /// 版权跳转Url
        /// </summary>
        public string BanVideoJumpUrl { get; set; }
    }

    /// <summary>
    /// 剧集信息组
    /// </summary>
    [AutoMap(typeof(VideoEpisodeGroup))]
    public class VideoEpisodeGroupDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 剧集组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 剧集组状态
        /// </summary>
        public EpisodeGroupStatus EpisodeGroupStatus { get; set; }

        /// <summary>
        /// 剧集组状态Str
        /// </summary>
        public string EpisodeGroupStatusStr => EpisodeGroupStatus.GetDisplayName();

        /// <summary>
        /// 剧集组类型
        /// </summary>
        public EpisodeGroupType EpisodeGroupType { get; set; }

        /// <summary>
        /// 剧集组类型Str
        /// </summary>
        public string EpisodeGroupTypeStr => EpisodeGroupType.GetDisplayName();

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越考前
        /// </remarks>
        public int OrderBy { get; set; }

        /// <summary>
        /// 剧集
        /// </summary>
        public List<VideoEpisodeDto> Episodes { get; set; }
    }

    /// <summary>
    /// 剧集
    /// </summary>
    [AutoMap(typeof(VideoEpisode))]
    public class VideoEpisodeDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 剧集Url
        /// </summary>
        public string EpisodeUrl { get; set; }

        /// <summary>
        /// 剧集名
        /// </summary>
        public string EpisodeName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越考前
        /// </remarks>
        public int OrderBy { get; set; }
    }

    /// <summary>
    /// 剧集信息组 扩展
    /// </summary>
    public static class VideoEpisodeGroupDtoExtension
    {
        /// <summary>
        /// 剧集组统一排序
        /// </summary>
        /// <returns></returns>
        public static List<VideoEpisodeGroupDto> OrderByExt(this IList<VideoEpisodeGroupDto> list)
        {
            foreach (var groupItem in list)
            {
                groupItem.Episodes = groupItem.Episodes.OrderByDescending(a => a.OrderBy)
                    .ThenBy(a => a.EpisodeName.Length)
                    .ThenBy(a => a.EpisodeName)
                    .ToList();
            }

            return list.OrderByDescending(a => a.OrderBy).ToList();
        }

        /// <summary>
        /// 剧集组统一排序
        /// </summary>
        /// <returns></returns>
        public static void OrderByExt(this VideoEpisodeGroupDto item)
        {
            item.Episodes = item.Episodes.OrderByDescending(a => a.OrderBy)
                 .ThenBy(a => a.EpisodeName.Length)
                 .ThenBy(a => a.EpisodeName)
                 .ToList();
        }
    }
}
