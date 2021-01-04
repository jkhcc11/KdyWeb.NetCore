using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 创建搜索配置 Dto
    /// </summary>
    public class GetPageParseInstanceDto
    {
        /// <summary>
        /// 搜索实例
        /// </summary>
        public IPageParseService<NormalPageParseOut, NormalPageParseInput> Instance { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public long ConfigId { get; set; }
    }
}
