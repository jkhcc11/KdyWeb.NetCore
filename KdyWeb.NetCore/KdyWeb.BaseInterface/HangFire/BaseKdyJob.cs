using KdyWeb.BaseInterface.KdyLog;

namespace KdyWeb.BaseInterface.HangFire
{
    /// <summary>
    /// 执行Job基类
    /// </summary>
    public abstract class BaseKdyJob<TInput>
    {
        protected IKdyLog KdyLog;
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
