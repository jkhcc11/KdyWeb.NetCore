using AutoMapper;
using KdyWeb.Entity.VideoConverts;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询转码订单列表dto
    /// </summary>
    [AutoMap(typeof(ConvertOrder))]
    public class QueryOrderListWithAdminDto : QueryMeOrderListDto
    {
        /// <summary>
        /// 创建用户名
        /// </summary>
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 修改用户名
        /// </summary>
        public string ModifyUserName { get; set; }
    }
}
