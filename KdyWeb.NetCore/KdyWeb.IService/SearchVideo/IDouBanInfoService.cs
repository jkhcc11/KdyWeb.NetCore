using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 豆瓣信息 服务接口
    /// </summary>
    public interface IDouBanInfoService : IKdyService
    {
        /// <summary>
        /// 创建豆瓣信息
        /// </summary>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        Task<KdyResult<CreateForSubjectIdDto>> CreateForSubjectIdAsync(string subjectId);

        /// <summary>
        /// 获取最新豆瓣信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetTop50DouBanInfoDto>>> GetTopDouBanInfoAsync(int topNumber = 50);

        /// <summary>
        /// 查询豆瓣信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryDouBanInfoDto>>> QueryDouBanInfoAsync(QueryDouBanInfoInput input);

        /// <summary>
        /// 获取豆瓣信息
        /// </summary>
        /// <param name="douBanInfoId">豆瓣信息Id</param>
        /// <returns></returns>
        Task<KdyResult<GetDouBanInfoForIdDto>> GetDouBanInfoForIdAsync(int douBanInfoId);

        /// <summary>
        /// 变更豆瓣信息状态
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ChangeDouBanInfoStatusAsync(ChangeDouBanInfoStatusInput input);

        /// <summary>
        /// 根据关键字创建豆瓣信息
        /// </summary>
        /// <remarks>
        ///  若未匹配则直接返回失败
        /// </remarks>
        /// <param name="keyWord">关键字</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        Task<KdyResult<CreateForSubjectIdDto>> CreateForKeyWordAsync(string keyWord, int year);

        /// <summary>
        /// 重试保存图片
        /// </summary>
        /// <remarks>
        /// 任务没保存成功时 手动再重写保存
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> RetrySaveImgAsync(int douBanInfoId);
    }
}
