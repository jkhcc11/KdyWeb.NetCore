using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.VerificationCode;

namespace KdyWeb.IService.VerificationCode
{
    /// <summary>
    /// 验证码服务 接口
    /// </summary>
    public interface IVerificationCodeService : IKdyService
    {
        /// <summary>
        /// 验证码生成
        /// </summary>
        /// <returns></returns>
        Task<string> GeneralCodeAsync();

        /// <summary>
        /// 验证码检查
        /// </summary>
        /// <param name="type">验证码类型</param>
        /// <param name="email">邮箱</param>
        /// <param name="userCode">用户验证码</param>
        /// <returns></returns>
        Task<KdyResult> CheckVerificationCodeAsync(VerificationCodeType type, string email, string userCode);
    }
}
