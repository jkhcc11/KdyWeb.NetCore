using Snowflake.Core;

namespace KdyWeb.BaseInterface.Repository
{
    /// <summary>
    /// Long Id生成 实现
    /// </summary>
    public class IdGenerateForLong : IIdGenerate<long>
    {
        private readonly IdWorker _worker;
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="workId">工作Id 1-31之间</param>
        /// <param name="datacenterId">中心Id  1-31之间</param>
        public IdGenerateForLong(long workId, long datacenterId)
        {
            _worker = new IdWorker(workId, datacenterId);
        }

        public long Create()
        {
            return _worker.NextId();
        }
    }
}
