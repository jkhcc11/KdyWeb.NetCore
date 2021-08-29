using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Winton.Extensions.Configuration.Consul;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// Consul配置中心扩展
    /// </summary>
    public static class ConsulConfigCenterExt
    {
        /// <summary>
        /// Consul地址
        /// </summary>
        public const string ConsulConfigUrl = "ConsulConfigUrl";

        /// <summary>
        /// 客户端名称
        /// </summary>
        /// <remarks>
        /// 同一台服务器部署多个时
        /// </remarks>
        public const string ConfigClientName = "ConfigClientName";

        /// <summary>
        /// 初始化配置中心
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="hostingContext"></param>
        /// <param name="consulUrl">Consul地址</param>
        /// <param name="consulKeys">支持多个配置文件</param>
        public static void InitConfigCenter(this IConfigurationBuilder configurationBuilder, HostBuilderContext hostingContext, string consulUrl, params string[] consulKeys)
        {
            foreach (var key in consulKeys)
            {
                configurationBuilder.AddConsul(
                    key,
                    options =>
                    {
                        options.ConsulConfigurationOptions = consulConfig =>
                        {
                            consulConfig.Address = new Uri(consulUrl);
                        };//Consul地址
                        options.Optional = true; //配置选项当前配置文件为可有可无
                        options.ReloadOnChange = true; //配置文件更新后重新加载
                        options.OnLoadException = exceptionContext =>
                        {
                            exceptionContext.Ignore = true; // 忽略异常
                        };

                    }
                );
            }

            // consul中加载的配置信息加载到Configuration对象，然后通过Configuration 对象加载项目中
            hostingContext.Configuration = configurationBuilder.Build();
        }
    }
}
