using KdyWeb.Entity.CloudParse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 服务器Cookie Map
    /// </summary>
    public class ServerCookieMap : KdyBaseMap<ServerCookie, long>
    {
        public ServerCookieMap() : base("CloudParse_ServerCookie")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<ServerCookie> builder)
        {

        }
    }
}
