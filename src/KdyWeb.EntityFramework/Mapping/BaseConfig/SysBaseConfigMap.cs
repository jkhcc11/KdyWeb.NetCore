using KdyWeb.Entity.BaseConfig;
using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 服务器Cookie Map
    /// </summary>
    public class SysBaseConfigMap : KdyBaseMap<SysBaseConfig, long>
    {
        public override void MapperConfigure(EntityTypeBuilder<SysBaseConfig> builder)
        {
            builder.Property(a => a.ImgUrl).HasMaxLength(VideoMain.UrlLength);
            builder.Property(a => a.TargetUrl).HasMaxLength(VideoMain.UrlLength);
            builder.Property(a => a.ConfigName).HasMaxLength(SysBaseConfig.ConfigNameLength);
            builder.Property(a => a.Remark).HasMaxLength(SysBaseConfig.RemarkLength);
        }
    }
}
