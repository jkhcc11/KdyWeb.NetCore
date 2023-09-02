using AutoMapper;
using KdyWeb.CloudParse.Out;
using KdyWeb.Utility;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 基础文件查询 Dto
    /// </summary>
    [AutoMap(typeof(BaseResultOut))]
    public class BaseCloudQueryFileDto : BaseResultOut
    {
        /// <summary>
        /// 文件名编码
        /// </summary>
        /// <remarks>
        /// 用于外链
        /// </remarks>
        public string NameEncode { get; set; }

        /// <summary>
        /// Id编码
        /// </summary>
        /// <remarks>
        /// 用于外链
        /// </remarks>
        public string IdEncode { get; set; }

        /// <summary>
        /// 播放器 路由路径
        /// </summary>
        /// <remarks>
        ///  eg： /pan-parse/ali/
        /// </remarks>
        public string ParseApiRoutePath { get; protected set; }

        /// <summary>
        /// json 路由路径
        /// </summary>
        /// <remarks>
        ///  eg： /pan-parse/ali/
        /// </remarks>
        public string ParseApiJsonPath { get; protected set; }

        /// <summary>
        /// 设置Id和名称
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <param name="fileName">文件名</param>
        /// <param name="extId">扩展Id 如果有</param>
        public void SetIdAndName(string fileId, string fileName, string extId = null)
        {
            var endSuffix = string.Empty;
            if (string.IsNullOrEmpty(extId) == false)
            {
                endSuffix = $"|{extId}";
            }

            IdEncode = ($"{fileId}{endSuffix}").StrToHex();
            NameEncode = ($"{fileName}{endSuffix}").StrToHex();
        }

        /// <summary>
        /// 设置路径信息
        /// </summary>
        /// <param name="playRoutePath">播放器前缀</param>
        /// <param name="jsonPath">json前缀</param>
        public void SetPathInfo(string playRoutePath, string jsonPath)
        {
            ParseApiRoutePath = playRoutePath;
            ParseApiJsonPath = jsonPath;
        }
    }
}
