using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.Job;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// 循环Url配置 服务接口
    /// </summary>
    public interface IRecurrentUrlConfigService : IKdyService
    {
        /// <summary>
        /// 创建循环Url配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateRecurrentUrlConfigAsync(CreateRecurrentUrlConfigInput input);

        /// <summary>
        /// 修改循环Url配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyRecurrentUrlConfigAsync(ModifyRecurrentUrlConfigInput input);

        /// <summary>
        /// 循环Url实现
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> RecurrentUrlAsync(RecurrentUrlJobInput input);
    }
}
