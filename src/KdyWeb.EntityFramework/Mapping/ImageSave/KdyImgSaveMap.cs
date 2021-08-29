using System;
using KdyWeb.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 图床关联 Map
    /// </summary>
    public class KdyImgSaveMap : KdyBaseMap<KdyImgSave, long>
    {
        public KdyImgSaveMap() : base("Kdy.ImgSave")
        {
        }

        public override void MapperConfigure(EntityTypeBuilder<KdyImgSave> builder)
        {
            builder.Property(a => a.Urls)
                .HasConversion(
                    a => string.Join(',', a),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
