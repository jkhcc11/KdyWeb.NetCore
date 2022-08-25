using System.Collections.Generic;

namespace KdyWeb.Dto.HttpApi.GameCheck
{
    /// <summary>
    /// 游戏检查配置
    /// </summary>
    public class GameCheckConfig
    {
        /// <summary>
        /// 实时便签version
        /// </summary>
        public string DailyNoteVersion { get; set; }

        /// <summary>
        /// 实时便签salt
        /// </summary>
        public string DailyNoteSalt { get; set; }

        /// <summary>
        /// BBS version
        /// </summary>
        public string BbsVersion { get; set; }

        /// <summary>
        /// BBS salt
        /// </summary>
        public string BbsSalt { get; set; }
    }
}
