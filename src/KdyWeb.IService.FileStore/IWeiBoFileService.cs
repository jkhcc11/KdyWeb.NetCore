using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;

namespace KdyWeb.IService.FileStore
{
    /// <summary>
    /// 微博上传 接口
    /// </summary>
    [Obsolete("2023已弃用")]
    public interface IWeiBoFileService : IKdyFileService<WeiBoFileInput>, IKdyService
    {
        /// <summary>
        /// 获取登录Cookie
        /// </summary>
        /// <returns></returns>
        Task<string> GetLoginCookie();
    }
}
