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
        /// 开始迁移
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

    }
}
