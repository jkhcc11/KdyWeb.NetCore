using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService.OldMigration
{
    /// <summary>
    /// 旧版影视迁移
    /// </summary>
    public interface IOldSysMainService : IKdyService
    {
        Task<KdyResult> OldToNew();
    }
}
