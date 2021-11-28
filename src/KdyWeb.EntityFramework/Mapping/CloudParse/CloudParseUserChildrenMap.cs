using KdyWeb.Entity.CloudParse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 云盘用户子账号 Map
    /// </summary>
    public class CloudParseUserChildrenMap : KdyBaseMap<CloudParseUserChildren, int>
    {
        public CloudParseUserChildrenMap() : base("CloudParse_UserChildren")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<CloudParseUserChildren> builder)
        {
        }
    }
}
