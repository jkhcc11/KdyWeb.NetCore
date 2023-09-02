using KdyWeb.BaseInterface;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 根据类型获取子账号列表
    /// </summary>
    public class GetSubAccountByTypeDto
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        /// <remarks>
        ///  子账号有别名 则显示别名 没有则显示用户名+序号
        /// </remarks>
        public string ShowName { get; set; }

        /// <summary>
        /// 查询Key
        /// </summary>
        /// <remarks>
        ///  直接用子账号Id
        /// </remarks>
        [JsonConverter(typeof(JsonConverterLong))]
        public long QueryValue { get; set; }
    }
}
