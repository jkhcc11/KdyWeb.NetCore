using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
namespace KdyWeb.Repository
{
    /// <summary>
    /// 通用仓储 实现
    /// </summary>
    /// <typeparam name="TEntity">数据库实体类</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    public class CommonRepository<TEntity, TKey> : KdyRepository<TEntity, TKey>
        where TEntity : class, IBaseKey<TKey>
        where TKey : struct
    {
    }

    /// <summary>
    /// Int通用仓储 实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CommonRepository<TEntity> : KdyRepository<TEntity, int>, IKdyRepository<TEntity>
        where TEntity : class, IBaseKey<int>
    {
    }
}
