using System.Collections.Generic;

namespace KdyWeb.Dto.HttpApi.AuthCenter
{
    /// <summary>
    /// 获取用户声明 Item
    /// </summary>
    public class GetUserClaimsResponseItem
    {
        /// <summary>
        /// 声明Id
        /// </summary>
        public int ClaimId { get; set; }
        
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 声明类型
        /// </summary>
        public string ClaimType { get; set; }
        
        /// <summary>
        /// 声明值
        /// </summary>
        public string ClaimValue { get; set; }
    }

    /// <summary>
    /// 获取用户声明 Response
    /// </summary>
    public class GetUserClaimsResponse : PageResponse
    {
        /// <summary>
        /// 声明列表
        /// </summary>
        public List<GetUserClaimsResponseItem> Claims { get; set; }
    }
}
