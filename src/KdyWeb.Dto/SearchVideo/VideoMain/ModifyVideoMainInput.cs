using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 更新影片主表信息 Input
    /// </summary>
    public class ModifyVideoMainInput
    {
        /// <summary>
        /// 影片Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        [StringLength(DouBanInfo.AkaLength)]
        public string Aka { get; set; }

        /// <summary>
        /// 影片状态
        /// </summary>
        [Required]
        public VideoMainStatus VideoMainStatus { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        [StringLength(DouBanInfo.VideoImgLength)]
        [Required]
        public string VideoImg { get; set; }

        /// <summary>
        /// 影片类型
        /// </summary>
        [Required]
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        [StringLength(DouBanInfo.VideoTitleLength)]
        [Required]
        public string KeyWord { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 影片来源url
        /// </remarks>
        [StringLength(VideoMain.UrlLength)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 源Url特征码
        /// </summary>
        [StringLength(VideoMain.VideoContentFeatureLength)]
        public string VideoContentFeature { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越展示靠前
        /// </remarks>
        [Required]
        public int OrderBy { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        [Required]
        public bool IsEnd { get; set; }

        /// <summary>
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        [StringLength(DouBanInfo.VideoGenresLength)]
        public string VideoGenres { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string VideoSummary { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        [StringLength(DouBanInfo.VideoCastsLength)]
        public string VideoCasts { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        [StringLength(DouBanInfo.VideoDirectorsLength)]
        public string VideoDirectors { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        [StringLength(DouBanInfo.VideoCountriesLength)]
        public string VideoCountries { get; set; }

        /// <summary>
        /// 解说Url
        /// </summary>
        [StringLength(VideoMain.UrlLength)]
        public string NarrateUrl { get; set; }

        /// <summary>
        /// 版权跳转Url
        /// </summary>
        [StringLength(VideoMain.UrlLength)]
        public string BanVideoJumpUrl { get; set; }

        /// <summary>
        /// 下载地址 多个换行
        /// </summary>
        [Obsolete("弃用，最新使用EpisodeGroup")]
        public string DownUrl { get; set; }

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
        [StringLength(VideoMain.UrlLength)]
        public string VideoInfoUrl { get; set; }

        /// <summary>
        /// 剧集组
        /// </summary>
        public List<ModifyVideoMainEpGroupItem> EpisodeGroup { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }
    }

    /// <summary>
    /// 剧集组
    /// </summary>
    public class ModifyVideoMainEpGroupItem
    {
        /// <summary>
        /// 剧集组类型
        /// </summary>
        public EpisodeGroupType EpisodeGroupType { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 剧集列表
        /// </summary>
        public List<UpdateEpisodeInput> Episodes { get; set; }
    }

    public class ModifyVideoMainInputProfile : Profile
    {
        public ModifyVideoMainInputProfile()
        {
            CreateMap<ModifyVideoMainInput, VideoMain>()
                .ForMember(a => a.Id, b => b.Ignore())
                .ForMember(a => a.EpisodeGroup, b => b.Ignore())
                .ForMember(a => a.Aka, opt =>
                {
                    opt.PreCondition(a => string.IsNullOrEmpty(a.Aka) == false);
                    opt.MapFrom(c => c.Aka);
                })
                .ForMember(a => a.KeyWord, opt =>
                {
                    opt.PreCondition(a => string.IsNullOrEmpty(a.KeyWord) == false);
                    opt.MapFrom(c => c.KeyWord);
                })
                .ForMember(a => a.VideoContentFeature, opt =>
                {
                    opt.PreCondition(a => string.IsNullOrEmpty(a.VideoContentFeature) == false);
                    opt.MapFrom(c => c.VideoContentFeature);
                })
                .ForMember(a => a.SourceUrl, opt =>
                {
                    opt.PreCondition(a => string.IsNullOrEmpty(a.SourceUrl) == false);
                    opt.MapFrom(c => c.SourceUrl);
                })
                .ForMember(a => a.VideoImg, opt =>
                {
                    opt.PreCondition(a => string.IsNullOrEmpty(a.VideoImg) == false);
                    opt.MapFrom(c => c.VideoImg);
                })
                  .ForMember(a => a.VideoYear, opt =>
                  {
                      opt.PreCondition(a => a.VideoYear > 1000);
                      opt.MapFrom(c => c.VideoYear);
                  })
                .ForPath(a => a.VideoMainInfo.VideoGenres, opt =>
                {
                    opt.Condition(a => string.IsNullOrEmpty(a.Source.VideoGenres) == false);
                    opt.MapFrom(c => c.VideoGenres);
                })
                .ForPath(a => a.VideoMainInfo.VideoDirectors, opt =>
                {
                    opt.Condition(a => string.IsNullOrEmpty(a.Source.VideoDirectors) == false);
                    opt.MapFrom(c => c.VideoDirectors);
                })
                .ForPath(a => a.VideoMainInfo.VideoCasts, opt =>
                {
                    opt.Condition(a => string.IsNullOrEmpty(a.Source.VideoCasts) == false);
                    opt.MapFrom(c => c.VideoCasts);
                })
                .ForPath(a => a.VideoMainInfo.VideoCountries, opt =>
                {
                    opt.Condition(a => string.IsNullOrEmpty(a.Source.VideoCountries) == false);
                    opt.MapFrom(c => c.VideoCountries);
                })
                .ForPath(a => a.VideoMainInfo.NarrateUrl, opt =>
                {
                    //opt.Condition(a => string.IsNullOrEmpty(a.Source.NarrateUrl) == false);
                    opt.MapFrom(c => c.NarrateUrl);
                })
                .ForPath(a => a.VideoMainInfo.VideoSummary, opt =>
                  {
                      //opt.Condition(a => string.IsNullOrEmpty(a.Source.VideoSummary) == false);
                      opt.MapFrom(c => c.VideoSummary);
                  })
                .ForPath(a => a.VideoMainInfo.BanVideoJumpUrl, opt =>
                {
                    //opt.Condition(a => string.IsNullOrEmpty(a.Source.BanVideoJumpUrl) == false);
                    opt.MapFrom(c => c.BanVideoJumpUrl);
                });
        }
    }

}
