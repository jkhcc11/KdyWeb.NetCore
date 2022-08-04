using System.Collections.Generic;
using System.Linq;
using KdyWeb.CloudParse.CloudParseEnum;

namespace KdyWeb.CloudParse.Extensions
{
    /// <summary>
    /// 文件类型扩展
    /// </summary>
    public static class CloudFileTypeExtension
    {
        /// <summary>
        /// 文件名转类型
        /// </summary>
        /// <returns></returns>
        public static CloudFileType FileNameToFileType(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return CloudFileType.Dir;
            }

            fileName = fileName.ToLower();

            var dic = new Dictionary<CloudFileType, List<string>>()
            {
                {
                    CloudFileType.Video,
                    new List<string>()
                    {
                        ".mp4", ".mkv", ".rmvb", ".avi"
                    }

                },
                {
                    CloudFileType.VideoTs,
                    new List<string>()
                    {
                        ".mkv", ".rmvb", ".avi", ".rm"
                    }

                },
                {
                    CloudFileType.Audio,
                    new List<string>()
                    {
                        ".mp3", ".flac", ".aac"
                    }

                },
                {
                    CloudFileType.Image,
                    new List<string>()
                    {
                        ".jpg", ".jpeg", ".bmp", ".gif"
                    }

                },
            };

            foreach (var item in dic)
            {
                if (item.Value.Any(a => fileName.EndsWith(a)))
                {
                    return item.Key;
                }
            }

            return CloudFileType.File;
        }
    }
}
