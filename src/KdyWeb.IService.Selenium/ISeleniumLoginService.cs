using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Selenium;

namespace KdyWeb.IService.Selenium
{
    /// <summary>
    /// Selenium模拟登录 服务接口
    /// </summary>
    public interface ISeleniumLoginService
    {
        /// <summary>
        /// 天翼云登录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<string>> LoginWithTyPersonAsync(LoginBySeleniumInput input);

        /// <summary>
        /// 天翼云H5登录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<string>> LoginWithTyH5Async(LoginBySeleniumInput input);

        /// <summary>
        /// 通用视频解析
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<string>> ParseVideoByUrlAsync(ParseVideoByUrlInput input);
    }
}
