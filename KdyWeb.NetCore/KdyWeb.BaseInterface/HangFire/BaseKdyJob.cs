using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyLog;
using Microsoft.Extensions.Logging;

namespace KdyWeb.BaseInterface.HangFire
{
    /// <summary>
    /// 执行Job基类
    /// </summary>
    public abstract class BaseKdyJob<TInput>
    {
        protected readonly IKdyLog KdyLog;

        protected BaseKdyJob(IKdyLog kdyLog)
        {
            KdyLog = kdyLog;
        }

        /// <summary>
        /// 具体执行
        /// </summary>
        public virtual void Execute(TInput input)
        {
            throw new Exception("未实现job同步执行方法");
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public virtual Task ExecuteAsync(TInput input)
        {
            throw new Exception("未实现job异步执行方法");
        }
    }
}
