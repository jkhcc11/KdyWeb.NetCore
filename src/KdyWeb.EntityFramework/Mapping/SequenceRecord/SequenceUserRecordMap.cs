using KdyWeb.Entity.SequenceRecord;

namespace KdyWeb.EntityFramework.Mapping.SequenceRecord
{
    /// <summary>
    /// 用户接龙列表 Map
    /// </summary>
    public class SequenceUserRecordMap : KdyBaseMap<SequenceUserRecord, long>
    {
        public SequenceUserRecordMap() : base("SequenceRecord_SequenceUserRecord")
        {

        }
    }
}
