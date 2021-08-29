using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 获取搜索结果Input
    /// </summary>
    public class KdyWebPageSearchInput: IKdyWebPageSearchInput
    {
        public string KeyWord { get; set; }
    }
}
