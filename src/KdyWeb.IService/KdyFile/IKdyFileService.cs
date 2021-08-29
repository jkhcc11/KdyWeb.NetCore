using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.KdyFile;

namespace KdyWeb.IService.KdyFile
{
    /// <summary>
    /// 文件存储 接口
    /// </summary>
    public interface IKdyFileService<T>
    where T : IBaseKdyFileInput
    {
        /// <summary>
        /// 根据Url上传
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<KdyFileDto>> PostFile(T input);
    }
}
