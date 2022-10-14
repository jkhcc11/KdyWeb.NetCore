using KdyWeb.Entity.VideoConverts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping.VideoConverts
{
    /// <summary>
    /// 影片管理者 Map
    /// </summary>
    public class VodManagerRecordMap : KdyBaseMap<VodManagerRecord, long>
    {
        public VodManagerRecordMap() : base("KdyTask_VodManagerRecord")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VodManagerRecord> builder)
        {

        }
    }
}
