using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService.Job
{
    /// <summary>
    /// Hangfire Job初始化 服务接口
    /// </summary>
    public interface IJobInitService : IKdyService
    {
        /// <summary>
        /// 初始化循环影片录入Job
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> InitRecurringVideoJobAsync();

        /// <summary>
        /// 初始化循环UrlJob
        /// </summary>
        /// <returns></returns>
        [Obsolete("废弃，以使用RecurrentUrlConfigService更新并创建")]
        Task<KdyResult> InitRecurrentUrlJobAsync();
    }
}
