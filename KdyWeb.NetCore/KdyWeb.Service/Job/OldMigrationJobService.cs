using System;
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
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewMain(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"迁移成功，pageInfo{input.ToJsonStr()}");
        }

        public async Task OldToNewUserAsync(OldMigrationJobInput input)
        {
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewUser(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"迁移成功，pageInfo{input.ToJsonStr()}");
        }

        public async Task OldToNewUserHistoryAsync(OldMigrationJobInput input)
        {
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewUserHistory(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"迁移成功，pageInfo{input.ToJsonStr()}");
        }

        public async Task OldToNewUserSubscribe(OldMigrationJobInput input)
        {
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewUserSubscribe(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"迁移成功，pageInfo{input.ToJsonStr()}");
        }

        public async Task OldToNewDanMu(OldMigrationJobInput input)
        {
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewDanMu(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"迁移成功，pageInfo{input.ToJsonStr()}");
        }

        public async Task OldToNewFeedBackInfo(OldMigrationJobInput input)
        {
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewFeedBackInfo(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"用户录入迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"用户录入迁移成功，pageInfo{input.ToJsonStr()}");
        }

        public async Task OldToNewSeries(OldMigrationJobInput input)
        {
            await Task.Delay(5000);

            if (input.Page <= 0 || input.PageSize <= 0)
            {
                throw new ArgumentNullException(nameof(OldMigrationJobInput));
            }

            var migration = await _oldSysMainService.OldToNewSeries(input.Page, input.PageSize);
            if (migration.IsSuccess == false)
            {
                KdyLog.Warn($"系列迁移出错，pageInfo{input.ToJsonStr()}");
                return;
            }

            KdyLog.Trace($"系列迁移成功，pageInfo{input.ToJsonStr()}");
        }
    }
}
