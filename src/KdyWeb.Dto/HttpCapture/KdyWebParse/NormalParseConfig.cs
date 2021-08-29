using KdyWeb.WebParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 通用站点解析配置
    /// </summary>
    public class NormalParseConfig : IKdyWebParseConfig
    {
        public string ApiHost { get; set; }
    }
}
