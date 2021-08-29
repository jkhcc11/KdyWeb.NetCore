using System;
using KdyWeb.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 二维码多图 Map
    /// </summary>
    public class QrImgSaveMap : KdyBaseMap<QrImgSave, long>
    {
        public override void MapperConfigure(EntityTypeBuilder<QrImgSave> builder)
        {
            builder.Property(a => a.ImgPath)
                .HasConversion(
                    a => string.Join(',', a),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
