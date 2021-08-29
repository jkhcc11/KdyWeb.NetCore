using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 异步方法Helper
    /// </summary>
    public class KdyAsyncHelper
    {
        /// <summary>
        /// 运行无返回异步方法
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Func<Task> action)
        {
            AsyncContext.Run(action);
        }

        /// <summary>
        /// 运行有返回参数异步方法
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="action">执行方法</param>
        /// <returns></returns>
        public static TResult Run<TResult>(Func<Task<TResult>> action)
        {
            return AsyncContext.Run(action);
        }
    }
}
