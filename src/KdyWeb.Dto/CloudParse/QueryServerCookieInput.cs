using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询服务器Cookie列表
    /// </summary>
    public class QueryServerCookieInput : BasePageInput
    {
        /// <summary>
        /// 状态
        /// </summary>
        [KdyQuery(nameof(ServerCookie.ServerCookieStatus), KdyOperator.Equal)]
        public ServerCookieStatus? ServerCookieStatus { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(ServerCookie.ServerIp), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
