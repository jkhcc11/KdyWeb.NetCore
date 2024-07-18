using System;
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
        /// 绑定豆瓣信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BindDouBanInfoAsync(MatchDouBanInfoInput input);

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

        /// <summary>
        /// 分页查询影视库(普通查询)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryVideoMainDto>>> QueryVideoByNormalAsync(QueryVideoByNormalInput input);

        /// <summary>
        /// 随机影片(普通查询)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<QueryVideoMainDto>>> RandVideoByNormalAsync(int count);

        /// <summary>
        /// 上|下架影片
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpAndDownVodAsync(long mainId);

        /// <summary>
        /// 通过豆瓣信息创建影片信息（新版）
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateForDouBanInfoNewAsync(CreateForDouBanInfoNewInput input);

        /// <summary>
        /// 通过豆瓣信息更新影片信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateVodForDouBanInfoAsync(UpdateVodForDouBanInfoInput input);

        /// <summary>
        /// 获取首页配置
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<HomeDataItem>>> GetHomeDataAsync();
    }
}
