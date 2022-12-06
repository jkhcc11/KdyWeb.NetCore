using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.GameDown;
using KdyWeb.IService.SearchVideo;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 游戏下载地址
    /// </summary>
    public class GameDownController : BaseManagerController
    {
        private readonly IGameDownService _gameDownService;

        public GameDownController(IGameDownService gameDownService)
        {
            _gameDownService = gameDownService;
        }

        /// <summary>
        /// 查询下载地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryGameDownListWithAdminDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryGameDownListWithAdminAsync([FromQuery] QueryGameDownListWithAdminInput input)
        {
            var result = await _gameDownService.QueryGameDownListWithAdminAsync(input);
            return Ok(result);
        }
    }
}
