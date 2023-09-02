using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Service;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using KdyWeb.Service.CloudParse.DiskCloudParse;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi.Controllers
{
    /// <summary>
    /// 天翼
    /// </summary>
    [CustomRoute("ty")]
    public class TyParseController : BaseCloudDiskApiController
    {
        private readonly ISubAccountService _subAccountService;
        private readonly ILoginUserInfo _loginUserInfo;

        public TyParseController(ISubAccountService subAccountService, ILoginUserInfo loginUserInfo)
        {
            _subAccountService = subAccountService;
            _loginUserInfo = loginUserInfo;
        }

        #region 个人
        /// <summary>
        /// 查询文件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("index")]
        public async Task<KdyResult<IList<BaseCloudQueryFileDto>>> QueryAliFileListAsync([FromQuery] BaseCloudQueryFileInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubAccountId);
            CheckSubAccountAuth(_loginUserInfo.GetUserId(), subAccount);

            var parseService = new TyPersonCloudParseService(new BaseConfigInput(subAccount.ShowName,
                subAccount.CookieInfo,
                subAccount.Id));
            var response = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                InputId = input.FileId,
                KeyWord = input.KeyWord,
            });
            if (response.IsSuccess == false)
            {
                return KdyResult.Error<IList<BaseCloudQueryFileDto>>(KdyResultCode.Error, response.Msg);
            }

            var result = response.Data.MapToListExt<BaseCloudQueryFileDto>();
            foreach (var itemDto in result)
            {
                itemDto.SetIdAndName(itemDto.ResultId, itemDto.ResultName);
                itemDto.SetPathInfo("/player-v2/ty-person/", "/api-v2/ty-person/");
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 批量修改文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("rename")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<KdyResult> BatchUpdateNameAsync(BaseBatchUpdateNameInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubInfo);
            CheckSubAccountAuth(_loginUserInfo.GetUserId(), subAccount);

            var parseService = new TyPersonCloudParseService(new BaseConfigInput(subAccount.ShowName,
                subAccount.CookieInfo, subAccount.Id));
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
            var aliCookieType = allCookieType.FirstOrDefault(a => a.BusinessFlag == CloudParseCookieType.TyPerson);
            if (aliCookieType == null)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取类型失败");
            }

            return KdyResult.Success<string>(aliCookieType.Id + "");
        }
        #endregion

        #region 企业
        /// <summary>
        /// 查询文件列表--企业
        /// </summary>
        /// <returns></returns>
        [HttpGet("crop-index")]
        public async Task<KdyResult<IList<BaseCloudQueryFileDto>>> QueryAliFileListWithCorpAsync([FromQuery] BaseCloudQueryFileInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubAccountId);
            CheckSubAccountAuth(_loginUserInfo.GetUserId(), subAccount);

            var parseService = new TyCropCloudParseService(new BaseConfigInput(subAccount.ShowName,
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
                itemDto.SetIdAndName(itemDto.ResultId, itemDto.ResultName, input.ExtId);
                itemDto.SetPathInfo("/player-v2/ty-crop/", "/api-v2/ty-crop/");
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 批量修改文件--企业
        /// </summary>
        /// <returns></returns>
        [HttpPost("crop-rename")]
        [ProducesResponseType(typeof(KdyResult), (int)HttpStatusCode.OK)]
        public async Task<KdyResult> BatchUpdateNameWithCorpAsync(BaseBatchUpdateNameInput input)
        {
            var subAccount = await _subAccountService.GetSubAccountCacheAsync(input.SubInfo);
            CheckSubAccountAuth(_loginUserInfo.GetUserId(), subAccount);

            var parseService = new TyCropCloudParseService(new BaseConfigInput(subAccount.ShowName,
                subAccount.CookieInfo, subAccount.Id));
            var request = input.FileItems
                .Where(a => a != null &&
                            a.OldName != a.NewName)
                .Select(a => new BatchUpdateNameInput()
                {
                    ExtId = input.ExtId,
                    FileId = a.FileId,
                    OldName = a.OldName,
                    NewName = a.NewName,
                })
                .ToList();
            var result = await parseService.BatchUpdateNameAsync(request);
            return result;
        }

        /// <summary>
        /// 获取当前网盘Cookie类型--企业
        /// </summary>
        /// <returns></returns>
        [HttpGet("crop-get-cookie-type-id")]
        public async Task<KdyResult<string>> GetCurrentCookieTypeWithCorpAsync()
        {
            var allCookieType = await _subAccountService.GetAllCookieTypeCacheAsync();
            var aliCookieType = allCookieType.FirstOrDefault(a => a.BusinessFlag == CloudParseCookieType.TyCrop);
            if (aliCookieType == null)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "获取类型失败");
            }

            return KdyResult.Success<string>(aliCookieType.Id + "");
        }
        #endregion
    }
}
