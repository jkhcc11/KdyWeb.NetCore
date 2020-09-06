using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.BaseInterface.KdyLog;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 基础服务定义 接口
    /// </summary>
    public interface IKdyService : IKdyScoped
    {
        /// <summary>
        /// 根据Key 获取配置信息
        /// </summary>
        /// <param name="key">配置Key</param>
        /// <returns></returns>
        T GetConfig<T>(string key);

        /// <summary>
        /// 获取日志实例
        /// </summary>
        /// <returns></returns>
        IKdyLog GetKdyLog();
    }
}
