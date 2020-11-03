using Microsoft.EntityFrameworkCore;

namespace KdyWeb.EntityFramework.ReadWrite
{
    /// <summary>
    /// 多数据库配置工厂 接口
    /// </summary>
    public interface IRwContextFactory
    {
        /// <summary>
        /// 获取上下文配置
        /// </summary>
        /// <returns></returns>
        DbContextOptions<ReadWriteContext> GetDbContext(ReadWrite rw);
    }

    /// <summary>
    /// 读写
    /// </summary>
    public enum ReadWrite
    {
        /// <summary>
        /// 读
        /// </summary>
        Read,

        /// <summary>
        /// 写
        /// </summary>
        Write
    }
}
