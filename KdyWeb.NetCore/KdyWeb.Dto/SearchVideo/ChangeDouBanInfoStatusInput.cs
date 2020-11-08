using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 变更豆瓣信息状态 Input
    /// </summary>
    public class ChangeDouBanInfoStatusInput
    {
        /// <summary>
        /// 豆瓣信息Id
        /// </summary>
        public int DouBanInfoId { get; set; }

        /// <summary>
        /// 豆瓣信息状态
        /// </summary>
        public DouBanInfoStatus DouBanInfoStatus { get; set; }
    }
}
