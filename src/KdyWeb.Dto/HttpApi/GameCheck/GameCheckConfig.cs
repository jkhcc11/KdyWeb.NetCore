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

        /// <summary>
        /// Bbs ua
        /// </summary>
        public string BbsUserAgent { get; set; } =
            "Mozilla/5.0 (Linux; Android 12; RMX3350 Build/SP1A.210812.016; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/97.0.4692.98 Mobile Safari/537.36";
    }
}
