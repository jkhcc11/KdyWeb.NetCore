using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片主表 服务接口
    /// </summary>
    public interface IVideoMainService : IKdyService
    {
        /// <summary>
        /// 通过豆瓣信息创建影片信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<CreateForDouBanInfoDto>> CreateForDouBanInfoAsync(CreateForDouBanInfoInput input);

        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetVideoDetailDto>> GetVideoDetailAsync(long keyId);

        /// <summary>
        /// 分页查询影视库
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVideoMainDto>>> QueryVideoMainAsync(QueryVideoMainInput input);

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateValueByFieldAsync(UpdateValueByFieldInput input);

        /// <summary>
        /// 批量删除影片
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input);

        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> MatchDouBanInfoAsync(MatchDouBanInfoInput input);

        /// <summary>
        /// 更新影片主表信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyVideoMainAsync(ModifyVideoMainInput input);

        /// <summary>
        /// 获取影片统计信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetCountInfoBySubtypeDto>>> GetCountInfoBySubtypeAsync(GetCountInfoBySubtypeInput input);

        /// <summary>
        /// 强制同步影片主表
        /// </summary>
        /// <param name="mainId">影片Id</param>
        /// <returns></returns>
        Task<KdyResult> ForceSyncVideoMainAsync(long mainId);

        /// <summary>
        /// 查询同演员影片列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<QuerySameVideoByActorDto>>> QuerySameVideoByActorAsync(QuerySameVideoByActorInput input);
    }
}
