using KdyWeb.BaseInterface.InterfaceFlag;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.WebParse;

namespace KdyWeb.IService.KdyWebParse
{
    /// <summary>
    /// 通用站点解析 服务接口
    /// </summary>
    public interface INormalWebParseService : IKdyWebParseService<KdyWebParseInput, KdyWebParseOut>, IKdyScoped
    {
    }
}
