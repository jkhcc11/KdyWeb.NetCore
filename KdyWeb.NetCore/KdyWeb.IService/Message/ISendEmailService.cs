using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Message;

namespace KdyWeb.IService.Message
{
    /// <summary>
    /// 发送Email服务 接口
    /// </summary>
    public interface ISendEmailService : IKdyService
    {
        /// <summary>
        /// 发送Email
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> SendEmailAsync(SendEmailInput input);
    }
}
