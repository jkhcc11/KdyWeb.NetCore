using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;

namespace KdyWeb.IService.SearchVideo
{
    /// <summary>
    /// 影片主表匹配信息相关 服务接口
    /// </summary>
    public interface IVideoMainMatchInfoService : IKdyService
    {
        /// <summary>
        /// 匹配豆瓣信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> MatchDouBanInfoAsync(long mainId);

        /// <summary>
        /// 匹配资源信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> MatchZyAsync(long mainId);

        /// <summary>
        /// 自动匹配并保存剧集
        /// </summary>
        /// <remarks>
        /// 1、豆瓣信息事先匹配好了，直接丢进来保存剧集
        /// 2、根据豆瓣名称+年份匹配影视库
        /// 3、全都匹配成功，更新剧集信息或者保存剧集信息
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult<string>> AutoMatchSaveEpAsync(AutoMatchSaveEpInput input);
    }
}
