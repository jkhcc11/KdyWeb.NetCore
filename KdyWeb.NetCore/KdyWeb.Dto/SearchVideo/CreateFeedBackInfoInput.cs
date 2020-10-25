﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 创建反馈信息 入参
    /// </summary>
    [AutoMap(typeof(FeedBackInfo), ReverseMap = true)]
    public class CreateFeedBackInfoInput
    {
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(FeedBackInfo.RemarkLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        /// <remarks>
        /// 1、反馈 -> 站点Url 发送邮件使用
        /// 2、录入 ->豆瓣Url、搜狗详情、百度百科等
        /// </remarks>
        [StringLength(FeedBackInfo.OriginalUrlLength)]
        [Required]
        public string OriginalUrl { get; set; }

        /// <summary>
        /// 反馈类型
        /// </summary>
        [Required]
        public UserDemandType DemandType { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        [StringLength(FeedBackInfo.VideoNameLength)]
        [Required]
        public string VideoName { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        [StringLength(FeedBackInfo.UserEmailLength)]
        [Required]
        public string UserEmail { get; set; }
    }
}
