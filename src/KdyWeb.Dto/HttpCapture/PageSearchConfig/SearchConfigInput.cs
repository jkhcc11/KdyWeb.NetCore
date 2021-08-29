using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 搜索配置 Input
    /// </summary>
    public class SearchConfigInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(PageSearchConfig.BaseHost), KdyOperator.Like)]
        [KdyQuery(nameof(PageSearchConfig.ServiceFullName), KdyOperator.Like)]
        [KdyQuery(nameof(PageSearchConfig.OtherHost), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
