using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.KdyUserNew
{
    public class KdyUserInvitationCode : BaseEntity<long>
    {
        public long UserId { get; set; }
    }
}
