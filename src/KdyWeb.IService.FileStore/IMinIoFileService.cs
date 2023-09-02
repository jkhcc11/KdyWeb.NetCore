using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;
using Minio;

namespace KdyWeb.IService.FileStore
{
    /// <summary>
    /// MinIO存储 服务接口
    /// </summary>
    public interface IMinIoFileService : IKdyFileService<MinIoFileInput>, IKdyService
    {
        /// <summary>
        /// 获取MinIo实例化客户端
        /// </summary>
        /// <returns></returns>
        MinioClient GetMinIoClient();
    }
}
