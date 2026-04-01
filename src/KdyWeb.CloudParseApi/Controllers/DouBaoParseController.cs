using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Service;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.CloudParse.DiskCloudParse;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 豆包
    /// </summary>
    [CustomRoute("dou")]
    public class DouBaoParseController : BaseCloudDiskApiController
    {
        private readonly ISubAccountService _subAccountService;
        private readonly ILoginUserInfo _loginUserInfo;

        public DouBaoParseController(ISubAccountService subAccountService, ILoginUserInfo loginUserInfo)
        {
            _subAccountService = subAccountService;
            _loginUserInfo = loginUserInfo;
        }

        /// <summary>
        /// 查询文件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("index")]
        public async Task<KdyResult<IList<BaseCloudQueryFileDto>>> QueryAliFileListAsync([FromQuery] BaseCloudQueryFileInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubAccountId);
            CheckSubAccountAuth(_loginUserInfo, subAccount);

            var parseService = new DouBaoParseService(new BaseConfigInput(subAccount.ShowName,
                subAccount.CookieInfo,
                subAccount.Id));
            var response = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                InputId = input.FileId,
                KeyWord = input.KeyWord,
                ExtData = input.ExtId
            });
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<IList<BaseCloudQueryFileDto>>(KdyResultCode.Error, response.Msg);
            }

            var result = response.Data.MapToListExt<BaseCloudQueryFileDto>();
            foreach (var itemDto in result)
            {
                itemDto.SetIdAndName(itemDto.ResultId, itemDto.ResultName, itemDto.ParentId);
                itemDto.SetPathInfoNew();
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 获取当前网盘Cookie类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-cookie-type-id")]
        public override async Task<KdyResult<string>> GetCurrentCookieTypeAsync()
        {
            var allCookieType = await _subAccountService.GetAllCookieTypeCacheAsync();
            var aliCookieType = allCookieType.FirstOrDefault(a => a.BusinessFlag == CloudParseCookieType.DouBao);
            if (aliCookieType == null)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取类型失败");
            }

            return KdyResult.Success<string>(aliCookieType.Id + "");
        }
    }
}
