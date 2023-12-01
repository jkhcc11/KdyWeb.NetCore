using KdyWeb.BaseInterface;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Input;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using KdyWeb.IService.CloudParse.DiskCloudParse;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析平台工厂
    /// </summary>
    public class DiskCloudParseFactory
    {
        private static readonly Dictionary<string, Type> AllParseServiceTypes;
        private static readonly Dictionary<string, string> AllParseServiceDownCacheKeyDic;
        static DiskCloudParseFactory()
        {
            // 寻找所有带有CloudParseServiceAttribute的类
            AllParseServiceTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes<CloudParseServiceAttribute>().Any())
                .ToDictionary(t => t.GetCustomAttribute<CloudParseServiceAttribute>()?.BusinessFlag,
                    t => t);

            AllParseServiceDownCacheKeyDic = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes<CloudParseServiceAttribute>().Any())
                .ToDictionary(
                    t => t.GetCustomAttribute<CloudParseServiceAttribute>()?.BusinessFlag,
                    t => t.GetCustomAttribute<CloudParseServiceAttribute>()?.DownCachePrefix);
        }

        public static IKdyCloudParseService CreateKdyCloudParseService(string businessFlag, long childUserId)
        {
            //一般用于清缓存
            if (AllParseServiceTypes.TryGetValue(businessFlag, out var serviceType))
            {
                return (IKdyCloudParseService)Activator.CreateInstance(serviceType, childUserId);
            }

            throw new KdyCustomException($"{nameof(CreateKdyCloudParseService)},未知业务标识: {businessFlag}");
        }

        public static IKdyCloudParseService CreateKdyCloudParseService(string businessFlag,
            BaseConfigInput baseConfigInput)
        {
            //实际调用
            if (AllParseServiceTypes.TryGetValue(businessFlag, out var serviceType))
            {
                return (IKdyCloudParseService)Activator.CreateInstance(serviceType, baseConfigInput);
            }

            throw new KdyCustomException($"{nameof(CreateKdyCloudParseService)},未知业务标识: {businessFlag}");
        }

        /// <summary>
        /// 业务标识转下载缓存前缀
        /// </summary>
        /// <returns></returns>
        public static string BusinessFlagToDownCachePrefix(string businessFlag)
        {
            if (AllParseServiceDownCacheKeyDic.TryGetValue(businessFlag, out var cachePrefix))
            {
                return cachePrefix;
            }

            throw new KdyCustomException("BusinessFlagToDownCachePrefix未知业务类型");
        }
    }
}
