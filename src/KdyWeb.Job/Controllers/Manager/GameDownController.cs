using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.GameDown;
using KdyWeb.IService.GameDown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 游戏下载地址
    /// </summary>
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.NormalPolicy)]
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
