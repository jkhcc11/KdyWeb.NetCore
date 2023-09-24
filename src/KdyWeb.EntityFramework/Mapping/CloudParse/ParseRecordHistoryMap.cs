using KdyWeb.Entity.CloudParse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 解析记录 Map
    /// </summary>
    public class ParseRecordHistoryMap : KdyBaseMap<ParseRecordHistory, long>
    {
        public ParseRecordHistoryMap() : base("CloudParse_ParseRecordHistory")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<ParseRecordHistory> builder)
        {

        }
    }
}
