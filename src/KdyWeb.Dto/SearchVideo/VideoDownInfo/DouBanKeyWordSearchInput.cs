using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 豆瓣关键字搜索
    /// </summary>
    public class DouBanKeyWordSearchInput:BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
