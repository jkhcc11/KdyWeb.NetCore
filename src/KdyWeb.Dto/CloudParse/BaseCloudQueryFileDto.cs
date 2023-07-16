using AutoMapper;
using KdyWeb.CloudParse.Out;

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
        /// 解析Api 路由路径
        /// </summary>
        /// <remarks>
        ///  eg： /pan-parse/ali/
        /// </remarks>
        public string ParseApiRoutePath { get; set; }
    }
}
