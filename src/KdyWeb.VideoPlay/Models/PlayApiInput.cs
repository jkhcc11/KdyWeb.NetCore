using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.VideoPlay.Models
{
    public class PlayApiInput
    {
        /// <summary>
        /// 剧集Id
        /// </summary>
        [Range(1, long.MaxValue,ErrorMessage = "无效剧集")]
        public long EpId { get; set; }
    }
}
