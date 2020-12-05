using KdyWeb.Entity.SearchVideo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 视频弹幕 Map
    /// </summary>
    public class VideoDanMuMap : KdyBaseMap<VideoDanMu, long>
    {

        public VideoDanMuMap() : base("VideoDanMu")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<VideoDanMu> builder)
        {

        }
    }
}
