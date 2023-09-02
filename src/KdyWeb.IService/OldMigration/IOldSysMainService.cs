using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService.OldMigration
{
    /// <summary>
    /// 旧版影视迁移
    /// </summary>
    [Obsolete("已升级部分，暂废弃")]
    public interface IOldSysMainService : IKdyService
    {
        Task<KdyResult> OldToNewMain(int page, int pageSize);

        Task<KdyResult> OldToNewUser(int page, int pageSize);

        Task<KdyResult> OldToNewUserHistory(int page, int pageSize);

        Task<KdyResult> OldToNewUserSubscribe(int page, int pageSize);

        Task<KdyResult> OldToNewDanMu(int page, int pageSize);

        Task<KdyResult> OldToNewFeedBackInfo(int page, int pageSize);

        Task<KdyResult> OldToNewSeries(int page, int pageSize);
    }
}
