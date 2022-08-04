using AutoMapper;
using KdyWeb.Entity.VideoConverts;

namespace KdyWeb.Dto.VideoConverts
{
    /// <summary>
    /// 查询任务列表(admin) Dto
    /// </summary>
    [AutoMap(typeof(VideoConvertTask))]
    public class QueryConvertTaskWithAdminDto : QueryConvertTaskWithNormalDto
    {
        /// <summary>
        /// 接单人
        /// </summary>
        public string TakeUserName { get; set; }
    }
}
