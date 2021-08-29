using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.BaseInterface.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.EntityFramework
{
    /// <summary>
    /// 多数据库配置工厂 接口
    /// </summary>
    public interface IRwContextFactory: IKdyScoped
    {
        /// <summary>
        /// 获取上下文配置
        /// </summary>
        /// <returns></returns>
        DbContextOptions<ReadWriteContext> GetDbContext(ReadWrite rw);
    }
}
