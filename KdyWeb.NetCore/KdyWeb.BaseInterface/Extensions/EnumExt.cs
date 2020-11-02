using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.BaseInterface.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExt
    {
        /// <summary>
        /// 获取枚举DisplayName
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumType)
        {
            var str = enumType.ToString();
            var field = enumType.GetType().GetField(str);
            var customAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0) return str;
            var da = (DisplayAttribute)customAttributes[0];
            return da.Name;
        }

        /// <summary>
        /// 获取枚举Description
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumType)
        {
            var str = enumType.ToString();
            var field = enumType.GetType().GetField(str);
            var customAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0) return str;
            var da = (DisplayAttribute)customAttributes[0];
            return da.Description;
        }
    }
}
