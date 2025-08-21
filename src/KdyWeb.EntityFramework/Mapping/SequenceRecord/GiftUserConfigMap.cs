using KdyWeb.Entity.SequenceRecord;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping.SequenceRecord
{
    /// <summary>
    /// 奖励用户配置 Map
    /// </summary>
    public class GiftUserConfigMap : KdyBaseMap<GiftUserConfig, long>
    {
        public GiftUserConfigMap() : base("SequenceRecord_GiftUserConfig")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<GiftUserConfig> builder)
        {
            //忽略
            builder.Ignore(a => a.IsChange);
        }
    }
}
