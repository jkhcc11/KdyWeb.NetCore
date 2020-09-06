using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;

namespace KdyWeb.IService.ImageSave
{
    /// <summary>
    /// 图床关联 服务接口
    /// </summary>
    public interface IKdyImgSaveService : IKdyService
    {
        /// <summary>
        /// 根据ImgId获取可用图片Url
        /// </summary>
        /// <param name="imgId">图片Id</param>
        /// <returns></returns>
        Task<string> GetImageByImgId(long imgId);
    }
}
