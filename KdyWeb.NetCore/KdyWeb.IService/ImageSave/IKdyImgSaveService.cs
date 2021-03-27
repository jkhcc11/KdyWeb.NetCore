using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyImg;

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
        /// <returns></returns>
        Task<KdyResult<string>> PostFileByUrl(PostFileByUrlInput input);

        /// <summary>
        /// 根据ImgId获取可用图片Url
        /// </summary>
        /// <param name="imgId">图片Id</param>
        /// <returns></returns>
        Task<string> GetImageByImgId(long imgId);

        /// <summary>
        /// 通过Byte上传
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<string>> PostFileByByteAsync(PostFileByByteInput input);
    }
}
