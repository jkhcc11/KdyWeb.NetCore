using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.InterfaceFlag;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.BaseInterface.Repository
{
    /// <summary>
    /// 工作单元 接口
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/xishuai/p/3750154.html
    /// </remarks>
    public interface IUnitOfWork : IKdyScoped, IDisposable
    {
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 异步保存更改
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 放弃所有更改
        /// </summary>
        void UnchangedAll();

        /// <summary>
        /// 获取当前数据库上下文
        /// </summary>
        /// <returns></returns>
        DbContext GetCurrentDbContext(ReadWrite rw);
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