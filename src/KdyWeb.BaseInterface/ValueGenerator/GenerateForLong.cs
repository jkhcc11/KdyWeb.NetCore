using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Core;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// Long值生成
    /// </summary>
    public class GenerateForLong : ValueGenerator<long>
    {
        private readonly IdWorker _worker;
        /// <summary>
        /// 实例化
        /// </summary>
        public GenerateForLong()
        {
            _worker = KdyBaseServiceProvider.ServiceProvide.GetService<IdWorker>();
        }

        public override long Next(EntityEntry entry)
        {
            return _worker.NextId();
        }

        public override bool GeneratesTemporaryValues => false;
    }
}
