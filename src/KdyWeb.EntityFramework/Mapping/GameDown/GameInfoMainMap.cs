using System;
using System.Collections.Generic;
using System.Linq;
using KdyWeb.Entity.GameDown;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 游戏主信息 Map
    /// </summary>
    public class GameInfoMainMap : KdyBaseMap<GameInfoMain, long>
    {
        public GameInfoMainMap() : base("GameDown_Main")
        {

        }

        public override void MapperConfigure(EntityTypeBuilder<GameInfoMain> builder)
        {
            builder.Property(a => a.ScreenCapture)
                .HasConversion(
                    a => string.Join(',', a),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            builder.Property(a => a.MovieList)
                .HasConversion(
                    entity => string.Join(',', entity),
                    dbEntity => dbEntity.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            builder.Property(a => a.DownList)
                .HasConversion(entity => entity.ToJsonStr(),
                    dbEntity => JsonConvert.DeserializeObject<List<GameInfoWithDownItem>>(dbEntity));
            builder.Property(a => a.ExtInfo)
                .HasConversion(entity => entity.ToJsonStr(),
                    dbEntity => JsonConvert.DeserializeObject<Dictionary<string, string>>(dbEntity));
        }
    }
}
