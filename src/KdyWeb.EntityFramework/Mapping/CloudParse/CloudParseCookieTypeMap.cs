using KdyWeb.Entity.CloudParse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 云盘用户子账号 Map
    /// </summary>
    public class CloudParseCookieTypeMap : KdyBaseMap<CloudParseCookieType, long>
    {
        public CloudParseCookieTypeMap() : base("CloudParse_CookieType")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<CloudParseCookieType> builder)
        {

        }
    }
}
