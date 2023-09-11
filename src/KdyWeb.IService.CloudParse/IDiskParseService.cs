using KdyWeb.BaseInterface.Service;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 解析 服务接口
    /// </summary>
    public interface IDiskParseService : IKdyService
    {
        /// <summary>
        /// 通用解析
        /// </summary>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        Task<KdyResult<CommonParseDto>> CommonParseAsync(string cachePrefix, string cloudParseType,
            object userInfo, string fileInfo, bool isTs, bool isName);

        /// <summary>
        /// 通用解析(校验信息)
        /// </summary>
        /// <param name="verifyInfo">来源ref 或者 api token</param>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        Task<KdyResult<CommonParseDto>> CommonParseAsync(string cachePrefix, string verifyInfo, string cloudParseType,
            object userInfo, string fileInfo, bool isTs, bool isName);

        /// <summary>
        /// 通用解析 服务器Cookie
        /// </summary>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        Task<KdyResult<CommonParseDto>> CommonParseWithServerCookieAsync(string cachePrefix, string cloudParseType,
            object userInfo, string fileInfo, bool isTs, bool isName);

        /// <summary>
        /// 通用解析 服务器Cookie  (校验信息)
        /// </summary>
        /// <param name="verifyInfo">来源ref 或者 api token</param>
        /// <param name="cachePrefix">缓存前缀</param>
        /// <param name="cloudParseType">盘解析类型  对应业务类型 <see cref="CloudParseCookieType.BusinessFlag"/></param>
        /// <param name="userInfo">用户信息  新版 subAccountId  旧版 xxx_id</param>
        /// <param name="fileInfo">文件信息 文件Id或文件名</param>
        /// <param name="isTs">是否切片（如果支持）</param>
        /// <param name="isName">文件信息是否为名称</param>
        /// <returns></returns>
        Task<KdyResult<CommonParseDto>> CommonParseWithServerCookieAsync(string cachePrefix, string verifyInfo, string cloudParseType,
            object userInfo, string fileInfo, bool isTs, bool isName);
    }
}
