using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Service;
using KdyWeb.CloudParse.CloudParseEnum;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.CloudParse.DiskCloudParse;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 盘115
    /// </summary>
    [CustomRoute("pan115")]
    public class Pan115ParseController : BaseCloudDiskApiController
    {
        private readonly ISubAccountService _subAccountService;
        private readonly ILoginUserInfo _loginUserInfo;

        public Pan115ParseController(ISubAccountService subAccountService, ILoginUserInfo loginUserInfo)
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

            var parseService = new Pan115CloudParseService(new BaseConfigInput(subAccount.ShowName,
                subAccount.CookieInfo,
                subAccount.Id));
            var response = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                InputId = input.FileId,
                KeyWord = input.KeyWord
            });
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<IList<BaseCloudQueryFileDto>>(KdyResultCode.Error, response.Msg);
            }

            var result = response.Data.MapToListExt<BaseCloudQueryFileDto>();
            foreach (var itemDto in result)
            {
                itemDto.SetIdAndName(itemDto.FileType != CloudFileType.Dir ?
                    itemDto.ParentId :
                    itemDto.ResultId, itemDto.ResultName);
                itemDto.SetPathInfoNew();
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 批量修改文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("rename")]
        public async Task<KdyResult> BatchUpdateNameAsync(BaseBatchUpdateNameInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubInfo);
            CheckSubAccountAuth(_loginUserInfo, subAccount);

            var parseService = new Pan115CloudParseService(new BaseConfigInput(subAccount.ShowName, subAccount.CookieInfo, subAccount.Id));
            var request = input.FileItems
                .Where(a => a != null &&
                            a.OldName != a.NewName)
                .Select(a => new BatchUpdateNameInput()
                {
                    FileId = a.FileId,
                    OldName = a.OldName,
                    NewName = a.NewName,
                })
                .ToList();
            var result = await parseService.BatchUpdateNameAsync(request);
            return result;
        }

        /// <summary>
        /// 获取当前网盘Cookie类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-cookie-type-id")]
        public override async Task<KdyResult<string>> GetCurrentCookieTypeAsync()
        {
            var allCookieType = await _subAccountService.GetAllCookieTypeCacheAsync();
            var aliCookieType = allCookieType.FirstOrDefault(a => a.BusinessFlag == CloudParseCookieType.Pan115);
            if (aliCookieType == null)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取类型失败");
            }

            return KdyResult.Success<string>(aliCookieType.Id + "");
        }
    }
}
