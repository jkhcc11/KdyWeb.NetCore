using System;
using System.Collections.Generic;
using System.Text;

namespace KdyWeb.Dto.HttpCapture.KdyCloudParse
{
    /// <summary>
    /// 盛天网盘下载参数
    /// </summary>
    public class StDownExtData
    {
        /// <summary>
        /// 用户浏览器
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        public string ParentId { get; set; }
    }
}
