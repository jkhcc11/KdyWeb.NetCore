using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyImg;

namespace KdyWeb.IService.FileStore
{
    /// <summary>
    /// 图床关联 服务接口
    /// </summary>
    public interface IKdyImgSaveService : IKdyService
    {
        /// <summary>
        /// 分页查询图床
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryKdyImgDto>>> QueryKdyImgAsync(QueryKdyImgInput input);

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateValueByFieldAsync(UpdateValueByFieldInput input);

        /// <summary>
        /// 批量删除图床
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input);

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
