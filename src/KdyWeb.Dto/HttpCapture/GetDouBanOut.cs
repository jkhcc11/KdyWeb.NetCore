﻿using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 豆瓣信息 出参
    /// </summary>
    public class GetDouBanOut
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="title">影片名</param>
        /// <param name="year">年</param>
        /// <param name="pic">海报</param>
        /// <param name="id">豆瓣Id</param>
        public GetDouBanOut(string title, int year, string pic, string id)
        {
            Title = title;
            Year = year;
            Pic = pic;
            Id = id;
        }

        /// <summary>
        /// 影片名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 影片类型
        /// </summary>
        public Subtype Subtype { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        public string Pic { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        public string Actors { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string Directors { get; set; }

        /// <summary>
        /// 影片类型 武侠，动作等
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string Tags { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// 简介描述
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 详情Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        /// <remarks>
        /// 多个以 ，隔开
        /// </remarks>
        public string Countries { get; set; }

        /// <summary>
        /// 评分人数
        /// </summary>
        public int RatingsCount { get; set; }

        /// <summary>
        /// 又名 
        /// </summary>
        /// <remarks>多个名称，逗号隔开</remarks>
        public string Aka { get; set; }

        /// <summary>
        /// 短评人数
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// 长评人数
        /// </summary>
        public int ReviewsCount { get; set; }

        #region 接口直接获取不到
        /// <summary>
        /// Imdb Url
        /// </summary>
        public string ImdbStr { get; set; }
        #endregion

    }

    public class GetDouBanOutProfile : Profile
    {
        public GetDouBanOutProfile()
        {
            CreateMap<GetDouBanOut, DouBanInfo>()
                .ForMember(a => a.VideoTitle, a => a.MapFrom(b => b.Title))
                .ForMember(a => a.VideoYear, a => a.MapFrom(b => b.Year))
                .ForMember(a => a.VideoImg, a => a.MapFrom(b => b.Pic))
                .ForMember(a => a.VideoCasts, a => a.MapFrom(b => b.Actors))
                .ForMember(a => a.VideoDirectors, a => a.MapFrom(b => b.Directors))
                .ForMember(a => a.VideoGenres, a => a.MapFrom(b => b.Tags))
                .ForMember(a => a.VideoRating, a => a.MapFrom(b => b.Rating))
                .ForMember(a => a.VideoSummary, a => a.MapFrom(b => b.Intro))
                .ForMember(a => a.VideoDetailId, a => a.MapFrom(b => b.Id))
                .ForMember(a => a.VideoCountries, a => a.MapFrom(b => b.Countries))
                .ForMember(a => a.Id, a => a.Ignore())
                .ReverseMap();
        }
    }
}
