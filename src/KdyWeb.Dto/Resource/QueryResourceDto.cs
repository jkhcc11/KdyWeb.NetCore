using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.BaseConfig;

namespace KdyWeb.Dto.Resource
{
    /// <summary>
    /// 查询资源列表
    /// </summary>
    [AutoMap(typeof(SysBaseConfig))]
    public class QueryResourceDto : CreatedUserDto<long>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ConfigTypeEnum ConfigType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatusEnum ConfigStatus { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 目标Url
        /// </summary>
        /// <remarks>
        ///  1、非通知使用
        ///  2、通知随意
        /// </remarks>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        /// <remarks>
        ///  仅Banner使用
        /// </remarks>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks>
        /// 通知必填
        /// </remarks>
        public string Remark { get; set; }
    }
}
