using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.BaseConfig
{
    /// <summary>
    /// 系统基础数据
    /// </summary>
    public class SysBaseConfig : BaseEntity<long>
    {
        /// <summary>
        /// 配置名长度
        /// </summary>
        public const int ConfigNameLength = 20;
        /// <summary>
        /// 备注长度
        /// </summary>
        public const int RemarkLength = 300;

        /// <summary>
        /// 系统基础数据
        /// </summary>
        /// <param name="configType">类型</param>
        /// <param name="configName">名称</param>
        /// <param name="targetUrl">目标Url</param>
        public SysBaseConfig(ConfigTypeEnum configType, string configName, string targetUrl)
        {
            ConfigType = configType;
            ConfigName = configName;
            TargetUrl = targetUrl;
            ConfigStatus = CommonStatusEnum.Ban;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public ConfigTypeEnum ConfigType { get; protected set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatusEnum ConfigStatus { get; protected set; }

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
        public string? ImgUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks>
        /// 通知是这个
        /// </remarks>
        public string? Remark { get; set; }

        public void SetType(ConfigTypeEnum configType)
        {
            ConfigType = configType;
        }

        public void Open()
        {
            ConfigStatus = CommonStatusEnum.Normal;
        }

        public void Ban()
        {
            ConfigStatus = CommonStatusEnum.Ban;
        }
    }
}
