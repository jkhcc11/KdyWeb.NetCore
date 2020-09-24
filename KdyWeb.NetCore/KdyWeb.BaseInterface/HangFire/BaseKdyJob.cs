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
        public abstract void Execute(TInput input);
    }
}
