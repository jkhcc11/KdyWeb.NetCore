using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 用户历史记录 Map
    /// </summary>
    public class UserHistoryMap : KdyBaseMap<UserHistory, long>
    {
        public UserHistoryMap() : base("UserHistory")
        {

        }
    }
}
