using System;

namespace KdyWeb.IService.CloudParse.DiskCloudParse
{
    /// <summary>
    /// 云盘解析标签 每个实现类标记用于自动创建
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CloudParseServiceAttribute : Attribute
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        public string BusinessFlag { get; }

        /// <summary>
        /// 下载缓存前缀
        /// </summary>
        public string DownCachePrefix { get; set; }

        public CloudParseServiceAttribute(string businessFlag)
        {
            BusinessFlag = businessFlag;
        }
    }
}
