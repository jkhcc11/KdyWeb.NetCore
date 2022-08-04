using System;
using System.Collections.Generic;
using System.Text;

namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 盛天离线下载列表
    /// </summary>
    public class StCloudDownloadListOut
    {
        /// <summary>
        /// 下载Url
        /// </summary>
        public string DownLoadUrl { get; set; }

        /// <summary>
        ///文件名
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public float Progress { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 离线状态
        /// </summary>
        public StCloudDownloadStatus Status { get; set; }

        /// <summary>
        /// 是否离线完成
        /// </summary>
        public bool IsDownSuccess => (int)Progress == 1;
    }

    /// <summary>
    /// 离线状态
    /// </summary>
    public enum StCloudDownloadStatus
    {
        /// <summary>
        /// 失效
        /// </summary>
        Lose = 3,

        /// <summary>
        /// 下载完成
        /// </summary>
        Finish = 4,

        /// <summary>
        /// 部分完成
        /// </summary>
        PartFinish = 5
    }
}
