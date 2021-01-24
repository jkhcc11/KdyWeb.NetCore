using System;
using KdyWeb.Entity;
using KdyWeb.Entity.HttpCapture;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 循环Url配置 Map
    /// </summary>
    public class RecurrentUrlConfigMap : KdyBaseMap<RecurrentUrlConfig, long>
    {
        public RecurrentUrlConfigMap() : base("RecurrentUrlConfig")
        {

        }
        public override void MapperConfigure(EntityTypeBuilder<RecurrentUrlConfig> builder)
        { 

        }
    }
}
