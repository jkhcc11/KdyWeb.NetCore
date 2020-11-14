

using System;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// 根据json节点路径获取json对象匹配值
        /// </summary>
        /// <param name="jObject">待获取的对象</param>
        /// <param name="path">节点路径  eg:data.url</param>
        /// <param name="key">对象数组里面的某个key</param>
        /// <param name="split">若节点是数组 分割字符</param>
        /// <param name="topArray">若是数组 取前几 默认为前三</param>
        /// <returns></returns>
        public static string GetValueExt(this JObject jObject, string path, string key = "", string split = ",", int topArray = 3)
        {
            var temp = jObject.SelectToken(path);
            if (temp == null)
            {
                return string.Empty;
            }

            if (!(temp is JArray tempArray))
            {
                //非数组
                return temp.ToString();
            }

            var stringBuilder = new StringBuilder();
            int i = 0;
            foreach (var token in tempArray)
            {
                if (i >= topArray)
                {
                    break;
                }

                //为空 就是 匹配字符串数组  不为空匹配对象数组
                stringBuilder.Append(key.IsEmptyExt() == false ? $"{token[key]}{split}" : $"{token}{split}");
                i++;
            }
            return stringBuilder.ToString().TrimEnd(',');

        }

        /// <summary>
        /// 动态更新实体类值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">实体类</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        public static void UpdateModelField<T>(this T model, string propertyName, object value)
        {
            var type = model.GetType();
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                return;
            }

            //动态更改值
            var changeV = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(model, changeV, null);
        }
    }
}
