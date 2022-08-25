using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpApi;
using KdyWeb.Dto.HttpApi.GameCheck.GenShin;

namespace KdyWeb.IService.HttpApi
{
    /// <summary>
    /// Genshin检查 服务接口
    /// </summary>
    public interface IGameCheckWithGenShinHttpApi : IKdyService
    {
        /// <summary>
        /// 查询实时便签
        /// </summary>
        /// <returns></returns>
        Task<GenShinResult<DailyNoteResult>> QueryDailyNoteAsync(QueryDailyNoteInput input);

        /// <summary>
        /// bbs签到
        /// </summary>
        /// <returns></returns>
        Task<GenShinResult<SignRewardResult>> BBsSignRewardAsync(BBsSignRewardInput input);

        /// <summary>
        /// 获取签到信息
        /// </summary>
        /// <returns></returns>
        Task<GenShinResult<QuerySignInfoResult>> QuerySignInfoAsync(QuerySignInfoInput input);

        /// <summary>
        /// 获取用户绑定角色信息
        /// </summary>
        /// <returns></returns>
        Task<GenShinResult<QueryUserBindInfoByCookieResult>> QueryUserBindInfoByCookieAsync(QueryUserBindInfoByCookieInput input);
    }
}
