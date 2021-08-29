using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.WebParse;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// CCTV解析 服务接口
    /// </summary>
    public interface ICctvWebParseService : IKdyWebParseService<KdyWebParseInput, KdyWebParseOut>, IKdyScoped
    {
    }
}
