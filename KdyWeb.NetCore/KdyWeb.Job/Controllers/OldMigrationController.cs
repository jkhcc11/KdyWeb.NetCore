using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyLog;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.OldMigration;
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
        [HttpGet("start")]
        public IActionResult OldToNew()
        {
            Task.Factory.StartNew(async () =>
            {
                int page = 1, pageSize = 300;
                while (true)
                {
                    try
                    {
                        var migration = await _oldSysMainService.OldToNew(page, pageSize);
                        if (migration.IsSuccess == false)
                        {
                            _kdyLog.Warn($"迁移出错，page:{page},pageSize{pageSize}");
                            break;
                        }
                        _kdyLog.Trace($"迁移成功，page:{page},pageSize{pageSize}");

                    }
                    catch (Exception ex)
                    {
                        _kdyLog.Error(ex, new Dictionary<string, object>()
                        {
                            {"OldMigration",$"page:{page},PageSize:{pageSize}"}
                        });

                    }
                    finally
                    {
                        page++;
                        await Task.Delay(60000);
                    }
                }

                _kdyLog.Trace($"迁移完成，page:{page},pageSize{pageSize}");
            });
            return Ok("后台任务运行中");
        }

    }
}
