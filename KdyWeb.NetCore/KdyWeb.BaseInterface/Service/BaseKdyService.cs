using System;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.BaseInterface.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 基础服务定义 实现
    /// </summary>
    public abstract class BaseKdyService : IKdyService
    {
        /// <summary>
        /// 统一日志
        /// </summary>
        protected readonly IKdyLog KdyLog;
        /// <summary>
        /// 统一配置
        /// </summary>
        protected readonly IConfiguration KdyConfiguration;
        /// <summary>
        /// 工作单元
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;
        /// <summary>
        /// 登录信息
        /// </summary>
        protected readonly ILoginUserInfo LoginUserInfo;

        protected BaseKdyService(IUnitOfWork unitOfWork)
        {
            //todo:UnitOfWork 用scope时 无法直接获取 先直接构造器注入 后面调整
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IKdyLog>();
            KdyConfiguration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
            LoginUserInfo = KdyBaseServiceProvider.ServiceProvide.GetService<ILoginUserInfo>();

            UnitOfWork = unitOfWork;
            //UnitOfWork = KdyBaseServiceProvider.HttpContextServiceProvide.GetService<IUnitOfWork>();

        }

        public virtual void Dispose()
        {
            UnitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
