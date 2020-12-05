﻿using System;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 根据剧集Id获取视频信息 Dto
    /// </summary>
    public class GetVideoInfoByEpIdDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="epId">剧集Id</param>
        /// <param name="playUrl">播放地址</param>
        public GetVideoInfoByEpIdDto(long epId, string playUrl)
        {
            EpId = epId;
            PlayUrl = playUrl;
        }

        /// <summary>
        /// 剧集Id(获取弹幕)
        /// </summary>
        public long EpId { get; set; }

        /// <summary>
        /// 播放地址
        /// </summary>
        public string PlayUrl { get; set; }

        /// <summary>
        /// 下一集Url
        /// </summary>
        public string NextEpUrl => $"/VideoPlay/Index/{NextEpId}";

        /// <summary>
        /// 下一集Id
        /// </summary>
        public long? NextEpId { get; set; }
    }
}
