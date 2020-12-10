using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.OldMigration;
using KdyWeb.Service.Job;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers
{
    /// <summary>
    /// 旧版迁移
    /// </summary>
    public class OldMigrationController : OldBaseApiController
    {
        private readonly IOldSysMainService _oldSysMainService;
        private readonly IKdyLog _kdyLog;

        public OldMigrationController(IOldSysMainService oldSysMainService, IKdyLog kdyLog)
        {
            _oldSysMainService = oldSysMainService;
            _kdyLog = kdyLog;
        }

        /// <summary>
        /// 影片
        /// </summary>
        /// <returns></returns>
        [HttpGet("start/{maxPage}")]
        public async Task<IActionResult> OldToNew(int maxPage)
        {
            if (maxPage <= 0 || maxPage >= 200)
            {
                maxPage = 1;
            }

            int page = 1, pageSize = 300;
            while (page < maxPage)
            {
                var jobInput = new OldMigrationJobInput(page, pageSize);
                BackgroundJob.Enqueue<OldMigrationJobService>(a => a.ExecuteAsync(jobInput));

                page++;
                await Task.Delay(500);
            }

            return Ok("后台任务运行中");
        }


        /// <summary>
        /// 用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("startUser/{maxPage}")]
        public async Task<IActionResult> OldToNewUserAsync(int maxPage)
        {
            if (maxPage <= 0 || maxPage >= 200)
            {
                maxPage = 1;
            }

            int page = 1, pageSize = 300;
            while (page < maxPage)
            {
                var jobInput = new OldMigrationJobInput(page, pageSize);
                BackgroundJob.Enqueue<OldMigrationJobService>(a => a.OldToNewUserAsync(jobInput));

                page++;
                await Task.Delay(500);
            }

            return Ok("后台任务运行中");
        }

        /// <summary>
        /// 用户历史记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("startUserHistory/{maxPage}")]
        public async Task<IActionResult> OldToNewUserHistoryAsync(int maxPage)
        {
            if (maxPage <= 0 || maxPage >= 200)
            {
                maxPage = 1;
            }

            int page = 1, pageSize = 300;
            while (page < maxPage)
            {
                var jobInput = new OldMigrationJobInput(page, pageSize);
                BackgroundJob.Enqueue<OldMigrationJobService>(a => a.OldToNewUserHistoryAsync(jobInput));

                page++;
                await Task.Delay(500);
            }

            return Ok("后台任务运行中");
        }

        /// <summary>
        /// 用户订阅
        /// </summary>
        /// <returns></returns>
        [HttpGet("startUserSubscribe/{maxPage}")]
        public async Task<IActionResult> OldToNewUserSubscribe(int maxPage)
        {
            if (maxPage <= 0 || maxPage >= 200)
            {
                maxPage = 1;
            }

            int page = 1, pageSize = 300;
            while (page < maxPage)
            {
                var jobInput = new OldMigrationJobInput(page, pageSize);
                BackgroundJob.Enqueue<OldMigrationJobService>(a => a.OldToNewUserSubscribe(jobInput));

                page++;
                await Task.Delay(500);
            }

            return Ok("后台任务运行中");
        }

        /// <summary>
        /// 弹幕迁移
        /// </summary>
        /// <returns></returns>
        [HttpGet("startDanMu/{maxPage}")]
        public async Task<IActionResult> OldToNewDanMu(int maxPage)
        {
            if (maxPage <= 0 || maxPage >= 200)
            {
                maxPage = 1;
            }

            int page = 1, pageSize = 300;
            while (page < maxPage)
            {
                var jobInput = new OldMigrationJobInput(page, pageSize);
                BackgroundJob.Enqueue<OldMigrationJobService>(a => a.OldToNewDanMu(jobInput));

                page++;
                await Task.Delay(500);
            }

            return Ok("后台任务运行中");
        }


        /// <summary>
        /// 用户录入迁移
        /// </summary>
        /// <returns></returns>
        [HttpGet("startFeedBack/{maxPage}")]
        public async Task<IActionResult> OldToNewFeedBackInfo(int maxPage)
        {
            if (maxPage <= 0 || maxPage >= 200)
            {
                maxPage = 1;
            }

            int page = 1, pageSize = 100;
            while (page < maxPage)
            {
                var jobInput = new OldMigrationJobInput(page, pageSize);
                BackgroundJob.Enqueue<OldMigrationJobService>(a => a.OldToNewFeedBackInfo(jobInput));

                page++;
                await Task.Delay(500);
            }

            return Ok("后台任务运行中");
        }
    }
}
