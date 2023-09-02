using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;

namespace KdyWeb.IService.FileStore
{
    /// <summary>
    /// 普通文件上传 接口
    /// </summary>
    public interface INormalFileService : IKdyFileService<NormalFileInput>, IKdyService
    {

    }
}
