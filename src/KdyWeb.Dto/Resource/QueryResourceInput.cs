using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.BaseConfig;

namespace KdyWeb.Dto.Resource
{
    /// <summary>
    /// 查询资源列表
    /// </summary>
    public class QueryResourceInput: BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(SysBaseConfig.ConfigName),KdyOperator.Like)]
        [KdyQuery(nameof(SysBaseConfig.TargetUrl), KdyOperator.Like)]
        [KdyQuery(nameof(SysBaseConfig.Remark), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
