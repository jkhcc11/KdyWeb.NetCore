using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 查询菜单列表
    /// </summary>
    public class QueryPageMenuInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
