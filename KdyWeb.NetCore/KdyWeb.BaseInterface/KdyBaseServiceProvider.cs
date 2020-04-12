using System;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 用户获取当前DI容器
    /// </summary>
    public class KdyBaseServiceProvider
    {
        /// <summary>
        /// 服务提供
        /// </summary>
        public static IServiceProvider ServiceProvide { get; set; }
    }
}
