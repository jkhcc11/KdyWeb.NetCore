using KdyWeb.Entity.CloudParse.Enum;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 变更用户状态
    /// </summary>
    public class ChangeStatusInput
    {
        /// <summary>
        /// 状态
        /// </summary>
        public ServerCookieStatus Status { get; set; } = ServerCookieStatus.Normal;
    }
}
