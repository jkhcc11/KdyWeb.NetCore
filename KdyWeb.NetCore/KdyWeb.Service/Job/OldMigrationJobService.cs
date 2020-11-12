using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Job;
using KdyWeb.IService.OldMigration;
using KdyWeb.Utility;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 旧数据迁移Job 服务
    /// </summary>
    public class OldMigrationJobService : BaseKdyJob<OldMigrationJobInput>
    {
        private readonly IOldSysMainService _oldSysMainService;
        public OldMigrationJobService(IKdyLog kdyLog, IOldSysMainService oldSysMainService) : base(kdyLog)
        {
            _oldSysMainService = oldSysMainService;
        }

        public override async Task ExecuteAsync(OldMigrationJobInput input)
        {
            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            try
            {
                var migration = await _oldSysMainService.OldToNew(input.Page, input.PageSize);
                if (migration.IsSuccess == false)
                {
                    KdyLog.Warn($"迁移出错，pageInfo{input.ToJsonStr()}");
                }

                KdyLog.Trace($"迁移成功，pageInfo{input.ToJsonStr()}");

            }
            catch (Exception ex)
            {
                KdyLog.Error(ex, new Dictionary<string, object>()
                        {
                            {"OldMigration",input}
                        });
                throw ex;
            }
        }
    }
}
