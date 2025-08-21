using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.EntityFramework.Mapping.SequenceRecord
{
    /// <summary>
    /// 接龙使用记录 Map
    /// </summary>
    public class SequenceUseRecordMap : KdyBaseMap<SequenceUseRecord, long>
    {
        public SequenceUseRecordMap() : base("SequenceRecord_SequenceUseRecord")
        {

        }

    }
}
