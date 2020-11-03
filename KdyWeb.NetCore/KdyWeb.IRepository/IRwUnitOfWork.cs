using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.EntityFramework.ReadWrite;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.IRepository
{
    /// <summary>
    /// 工作单元 接口
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/xishuai/p/3750154.html
    /// </remarks>
    public interface IRwUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 获取当前数据库上下文
        /// </summary>
        /// <returns></returns>
        DbContext GetCurrentDbContext(ReadWrite rw);
    }
}