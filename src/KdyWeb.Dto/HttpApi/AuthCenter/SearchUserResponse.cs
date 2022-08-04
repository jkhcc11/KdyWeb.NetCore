using System.Collections.Generic;

namespace KdyWeb.Dto.HttpApi.AuthCenter
{
    /// <summary>
    /// 搜索用户 ResponseItem
    /// </summary>
    public class SearchUserResponseItem
    {
        /// <summary>
        /// 用户Id　　
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }

    /// <summary>
    /// 搜索用户 Response
    /// </summary>
    public class SearchUserResponse : PageResponse
    {
        public List<SearchUserResponseItem> Users { get; set; }
    }
}
