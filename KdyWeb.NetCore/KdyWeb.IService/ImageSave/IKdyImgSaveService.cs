using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService.ImageSave
{
    /// <summary>
    /// 图床关联 服务接口
    /// </summary>
    public interface IKdyImgSaveService : IKdyService
    {
        /// <summary>
        /// 通过Url上传
        /// </summary>
        /// <param name="imgUrl">图片Url</param>
        /// <returns></returns>
        Task<KdyResult<string>> PostFileByUrl(string imgUrl);

        /// <summary>
        /// 根据ImgId获取可用图片Url
        /// </summary>
        /// <param name="imgId">图片Id</param>
        /// <returns></returns>
        Task<string> GetImageByImgId(long imgId);
    }
}
