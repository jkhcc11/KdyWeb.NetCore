using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.Selenium;

namespace KdyWeb.IService.Selenium
{
    /// <summary>
    /// 通过二维码登录
    /// </summary>
    public interface ILoginByQrService
    {
        /// <summary>
        /// 阿里云盘Qr 初始化
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QrLoginInitWithAliOut>> QrLoginInitWithAliAsync();

        /// <summary>
        /// 阿里云盘qr获取token
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<string>> QrLoginGetTokenWithAliAsync(QrLoginGetTokenInput input);
    }
}
