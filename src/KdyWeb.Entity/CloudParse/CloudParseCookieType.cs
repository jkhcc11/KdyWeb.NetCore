using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘Cookie类型
    /// </summary>
    public class CloudParseCookieType : BaseEntity<long>
    {
        public const int ShowTextLength = 50;

        /// <summary>
        /// 显示文案
        /// </summary>
        public string ShowText { get; set; }
    }
}
