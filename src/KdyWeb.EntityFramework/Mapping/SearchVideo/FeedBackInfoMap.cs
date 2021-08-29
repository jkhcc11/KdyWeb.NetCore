using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 反馈信息及录入 Map
    /// </summary>
    public class FeedBackInfoMap : KdyBaseMap<FeedBackInfo, int>
    {
        public FeedBackInfoMap() : base("FeedBackInfo")
        {

        }
    }
}
