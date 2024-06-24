using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.BaseConfig;
using KdyWeb.Entity.SearchVideo;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.Resource
{
    /// <summary>
    /// 创建资源
    /// </summary>
    public class CreateAndUpdateResourceInput
    {
        /// <summary>
        /// key
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [EnumDataType(typeof(ConfigTypeEnum))]
        public ConfigTypeEnum ConfigType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EnumDataType(typeof(CommonStatusEnum))]
        public CommonStatusEnum ConfigStatus { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(SysBaseConfig.ConfigNameLength)]
        public string ConfigName { get; set; }

        /// <summary>
        /// 目标Url
        /// </summary>
        /// <remarks>
        ///  1、非通知使用
        ///  2、通知随意
        /// </remarks>
        [StringLength(VideoMain.UrlLength)]
        public string TargetUrl { get; set; }

        /// <summary>
        /// 海报
        /// </summary>
        /// <remarks>
        ///  仅Banner使用
        /// </remarks>
        [StringLength(VideoMain.UrlLength)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks>
        /// 通知必填
        /// </remarks>
        [StringLength(SysBaseConfig.RemarkLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }
    }
}
