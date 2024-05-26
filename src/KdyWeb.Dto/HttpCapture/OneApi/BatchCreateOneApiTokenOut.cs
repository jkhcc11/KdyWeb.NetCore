using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 批量创建Token
    /// </summary>
    public class BatchCreateOneApiTokenOut
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Key { get; set; }

        public string Token => "sk-" + Key;
    }
}
