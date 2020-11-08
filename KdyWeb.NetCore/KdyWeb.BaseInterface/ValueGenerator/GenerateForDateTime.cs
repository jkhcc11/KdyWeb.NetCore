using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 时间生成
    /// </summary>
    public class GenerateForDateTime : ValueGenerator<DateTime>
    {
        public override DateTime Next(EntityEntry entry)
        {
            return DateTime.Now;
        }

        public override bool GeneratesTemporaryValues => false;
    }
}
