using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// OneApi 服务接口
    /// </summary>
    public interface IOneApiService : IKdyService
    {
        /// <summary>
        /// 批量创建Token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<KdyResult<List<BatchCreateOneApiTokenOut>>> BatchCreateOneApiTokenAsync(BatchCreateOneApiTokenInput input);
    }
}
