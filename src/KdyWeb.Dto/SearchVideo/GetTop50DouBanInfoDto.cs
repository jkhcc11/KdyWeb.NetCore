﻿using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 获取最新50条豆瓣信息 Dto
    /// </summary>
    [AutoMap(typeof(DouBanInfo))]
    public class GetTop50DouBanInfoDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string VideoTitle { get; set; }

        /// <summary>
        /// 豆瓣信息状态
        /// </summary>
        public DouBanInfoStatus DouBanInfoStatus { get; set; }

        /// <summary>
        /// 豆瓣信息状态Str
        /// </summary>
        public string DouBanInfoStatusStr => DouBanInfoStatus.GetDisplayName();

        /// <summary>
        /// 详情Id
        /// </summary>
        public string VideoDetailId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

    }
}
