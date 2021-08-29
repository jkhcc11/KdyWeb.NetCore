using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 用户订阅 Map
    /// </summary>
    public class UserSubscribeMap : KdyBaseMap<UserSubscribe, long>
    {
        public UserSubscribeMap() : base("UserSubscribe")
        {

        }
    }
}
