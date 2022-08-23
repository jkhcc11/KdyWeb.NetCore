using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
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
        /// <param name="uid">用户Id</param>
        /// <param name="cookie">Cookie</param>
        /// <param name="server">
        /// 服务器  cn_gf01 国服天空岛
        /// </param>
        /// <returns></returns>
        Task<GenShinResult<DailyNoteResult>> QueryDailyNote(string uid, string cookie, string server = "cn_gf01");
    }
}
