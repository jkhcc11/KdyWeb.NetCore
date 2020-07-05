

using Newtonsoft.Json;

namespace KdyWeb.Utility
{
    /// <summary>
    /// 对象扩展
    /// </summary>
    public static class ObjectExt
    {
        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <returns></returns>
        public static string ToJsonStr(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(obj);
        }
    }
}
