using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 查询前端可搜索配置
    /// </summary>
    [AutoMap(typeof(PageSearchConfig))]
    public class QueryShowPageConfigDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 站点名
        /// </summary>
        public string HostName { get; set; }
    }
}
