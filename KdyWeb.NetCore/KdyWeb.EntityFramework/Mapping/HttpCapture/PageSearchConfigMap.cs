using System;
using KdyWeb.Entity;
using KdyWeb.Entity.HttpCapture;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 站点页面搜索配置 Map
    /// </summary>
    public class PageSearchConfigMap : KdyBaseMap<PageSearchConfig, long>
    {
        public PageSearchConfigMap() : base("PageSearchConfig")
        {

        }
        public override void MapperConfigure(EntityTypeBuilder<PageSearchConfig> builder)
        {
            builder.Property(a => a.NameAttr)
                .HasConversion(
                    a => string.Join(',', a),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            builder.Property(a => a.ImgAttr)
                .HasConversion(
                    a => string.Join(',', a),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
