using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.KdyHttp;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 循环Url配置 服务实现
    /// </summary>
    public class RecurrentUrlConfigService : BaseKdyService, IRecurrentUrlConfigService
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        private readonly IKdyRepository<RecurrentUrlConfig, long> _recurrentUrlConfigRepository;

        public RecurrentUrlConfigService(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon, IKdyRepository<RecurrentUrlConfig, long> recurrentUrlConfigRepository) : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
            _recurrentUrlConfigRepository = recurrentUrlConfigRepository;
        }

        /// <summary>
        /// 创建循环Url配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateRecurrentUrlConfigAsync(CreateRecurrentUrlConfigInput input)
        {
            var exit = await _recurrentUrlConfigRepository.GetAsNoTracking()
                .AnyAsync(a => a.RequestUrl == input.RequestUrl);
            if (exit)
            {
                return KdyResult.Error(KdyResultCode.Error, $"创建循环Url配置失败，已存在{input.RequestUrl}");
            }

            var dbInput = input.MapToExt<RecurrentUrlConfig>();
            await _recurrentUrlConfigRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 修改循环Url配置
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyRecurrentUrlConfigAsync(ModifyRecurrentUrlConfigInput input)
        {
            var dbUrlConfig = await _recurrentUrlConfigRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (dbUrlConfig == null)
            {
                return KdyResult.Error(KdyResultCode.Error, $"修改循环Url配置失败，无效{input.Id}");
            }

            input.MapToPartExt(dbUrlConfig);
            _recurrentUrlConfigRepository.Update(dbUrlConfig);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 循环Url实现
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> RecurrentUrlAsync(RecurrentUrlJobInput input)
        {
            var reqInput = new KdyRequestCommonInput(input.RequestUrl, input.HttpMethod.ToHttpMethod())
            {
                Referer = input.Referer,
                Cookie = input.Cookie,
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = input.PostData
                }
            };
            var reqResult = await _kdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error(KdyResultCode.SystemError, $"循环Url请求失败，{reqResult.ErrMsg} 等待重试");
            }

            if (string.IsNullOrEmpty(input.MsgXpath) ||
                string.IsNullOrEmpty(input.SuccessFlag))
            {
                return KdyResult.Error(KdyResultCode.Error, "循环Url请求失败，未知关键字");
            }

            var info = reqResult.Data.GetHtmlNodeByXpath(input.MsgXpath)?.InnerText;
            if (string.IsNullOrEmpty(info) ||
                info.Contains(input.SuccessFlag) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, $"循环Url请求失败，提取成功标识失败。{info}");
            }

            return KdyResult.Success();
        }
    }
}
