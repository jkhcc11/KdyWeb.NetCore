using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;

namespace KdyWeb.IService.KdyFile
{
    /// <summary>
    /// 微博上传 接口
    /// </summary>
    public interface IWeiBoFileService : IKdyFileService<BaseKdyFileInput>, IKdyService
    {
        /// <summary>
        /// 获取登录Cookie
        /// </summary>
        /// <returns></returns>
        Task<string> GetLoginCookie();
    }
}
