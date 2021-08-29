using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 获取反馈统计信息 Dto
    /// </summary>
    public class GetCountInfoDto
    {
        /// <summary>
        /// 反馈类型
        /// </summary>
        public UserDemandType DemandType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public long Count { get; set; }
    }
}
