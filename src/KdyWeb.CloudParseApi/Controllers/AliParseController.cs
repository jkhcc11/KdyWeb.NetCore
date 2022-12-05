using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.CloudParse.Input;
using KdyWeb.CloudParse.Out;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.HttpCapture.KdyCloudParse;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 阿里云盘
    /// </summary>
    [Route("AliParse")]
    public class AliParseController : BaseApiController
    {
        private readonly ISubAccountService _subAccountService;
        private readonly ILoginUserInfo _loginUserInfo;
        public AliParseController(ISubAccountService subAccountService, ILoginUserInfo loginUserInfo)
        {
            _subAccountService = subAccountService;
            _loginUserInfo = loginUserInfo;
        }

        /// <summary>
        /// 查询文件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("index")]
        [ProducesResponseType(typeof(KdyResult<List<BaseResultOut>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryAliFileListAsync([FromQuery] BaseCloudQueryFileInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubInfo);
            CheckSubAccountAuth(_loginUserInfo.GetUserId(), CloudParseCookieType.AliDrive, subAccount);

            var parseService = new AliYunCloudParseService(new BaseConfigInput(subAccount.ShowName, subAccount.CookieInfo, subAccount.Id));
            var result = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                InputId = input.FileId,
                KeyWord = input.KeyWord
            });
            return Ok(result);
        }

        /// <summary>
        /// 批量修改文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("rename")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BatchUpdateNameAsync(BaseBatchUpdateNameInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubInfo);
            CheckSubAccountAuth(_loginUserInfo.GetUserId(), CloudParseCookieType.AliDrive, subAccount);

            var parseService = new AliYunCloudParseService(new BaseConfigInput(subAccount.ShowName, subAccount.CookieInfo, subAccount.Id));
            var request = input.FileItems
                .Where(a => a.OldName != a.NewName)
                .Select(a => new BatchUpdateNameInput()
                {
                    FileId = a.FileId,
                    OldName = a.OldName,
                    NewName = a.NewName,
                })
                .ToList();
            var result = await parseService.BatchUpdateNameAsync(request);
            return Ok(result);
        }
    }
}
