using System;
using System.Net.Http;
using KdyWeb.BaseInterface.Extensions;

namespace KdyWeb.Entity.HttpCapture
{
    /// <summary>
    /// 搜索请求方法
    /// </summary>
    public enum ConfigHttpMethod : byte
    {
        /// <summary>
        /// Get
        /// </summary>
        Get = 0,

        /// <summary>
        /// Post
        /// </summary>
        Post = 1
    }

    /// <summary>
    /// 搜索请求方法 扩展
    /// </summary>
    public static class ConfigHttpMethodExt
    {
        /// <summary>
        /// ConfigHttpMethod->HttpMethod
        /// </summary>
        /// <returns></returns>
        public static HttpMethod ToHttpMethod(this ConfigHttpMethod method)
        {
            switch (method)
            {
                case ConfigHttpMethod.Post:
                    {
                        return HttpMethod.Post;
                    }
                case ConfigHttpMethod.Get:
                    {
                        return HttpMethod.Get;
                    }
                default:
                    {
                        throw new ArgumentNullException($"不支持该方法,{method.GetDisplayName()}");
                    }
            }
        }
    }
}
