using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using System.Threading.Tasks;
using KdyWeb.Dto.HttpCapture;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// 腾讯文档WebApi 服务接口
    /// </summary>
    public interface ITxDocWebService : IKdyService
    {
        /// <summary>
        /// 获取腾讯文档接龙列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetSequenceRecordsOut>> GetSequenceRecordsAsync(GetSequenceRecordsInput input);
    }
}
