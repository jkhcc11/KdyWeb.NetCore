using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.VerificationCode;
using KdyWeb.IService.VerificationCode;
using Microsoft.Extensions.Caching.Distributed;

namespace KdyWeb.Service.VerificationCode
{
    /// <summary>
    /// 验证码服务 实现
    /// </summary>
    public class VerificationCodeService : BaseKdyService, IVerificationCodeService
    {
        private readonly IDistributedCache _distributedCache;

        public VerificationCodeService(IUnitOfWork unitOfWork, IDistributedCache distributedCache) : base(unitOfWork)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 验证码生成
        /// </summary>
        /// <returns></returns>
        public async Task<string> GeneralCodeAsync()
        {
            await Task.CompletedTask;
            var rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000, 999999) + "";
        }

        /// <summary>
        /// 验证码检查
        /// </summary>
        /// <param name="type">验证码类型</param>
        /// <param name="email">邮箱</param>
        /// <param name="userCode">用户验证码</param>
        /// <returns></returns>
        public async Task<KdyResult> CheckVerificationCodeAsync(VerificationCodeType type, string email, string userCode)
        {
            var cacheKey = type.GetVerificationCodeCacheKey(email);
            var cacheV = await _distributedCache.GetStringAsync(cacheKey);
            if (string.IsNullOrWhiteSpace(cacheV))
            {
                return KdyResult.Error(KdyResultCode.Error, "验证码校验异常");
            }

            var result= cacheV == userCode ? KdyResult.Success() : KdyResult.Error(KdyResultCode.Error, "验证码错误");
            if (result.IsSuccess)
            {
                await _distributedCache.RemoveAsync(cacheKey);
            }

            return result;
        }
    }
}
