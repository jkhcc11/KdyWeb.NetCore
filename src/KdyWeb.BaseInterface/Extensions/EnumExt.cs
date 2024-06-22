using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            if (field == null)
            {
                return string.Empty;
            }

            var customAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0) return str;
            var da = (DisplayAttribute)customAttributes[0];
            return da.Name ?? string.Empty;
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
            if (field == null)
            {
                return string.Empty;
            }

            var customAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0) return str;
            var da = (DisplayAttribute)customAttributes[0];
            return da.Description ?? string.Empty;
        }

        /// <summary>
        /// 获取枚举列表以及Display
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<(T EnumValue, string DisplayName)> GetEnumList<T>(
            IEnumerable<T>? includeValues = null,
            IEnumerable<T>? excludeValues = null) where T : Enum
        {
            var includeSet = includeValues != null ? new HashSet<T>(includeValues) : null;
            var excludeSet = excludeValues != null ? new HashSet<T>(excludeValues) : new HashSet<T>();

            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                if ((includeSet == null || includeSet.Contains(value)) && !excludeSet.Contains(value))
                {
                    yield return (value, value.GetDisplayName());
                }
            }
        }
    }
}
