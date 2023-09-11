using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 解析系统 服务接口
    /// </summary>
    public interface IParseSystemService : IKdyService
    {
        /// <summary>
        /// 影片发送入库
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ParseVodSendAsync(ParseVodSendInput input);
    }
}
