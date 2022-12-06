using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.Utility;

namespace KdyWeb.Entity.GameDown
{
    /// <summary>
    /// 游戏主信息
    /// </summary>
    public class GameInfoMain : BaseEntity<long>
    {
        public const int GameNameLength = 200;
        public const int GameVersionLength = 50;
        public const int GameUrlLength = 300;
        public const int UserHashLength = 50;

        /// <summary>
        /// 游戏主信息
        /// </summary>
        /// <param name="sourceMd5">特征码</param>
        /// <param name="gameName">游戏名</param>
        /// <param name="gameSize">大小</param>
        /// <param name="gameVersion">版本</param>
        /// <param name="gameCovert">封面</param>
        /// <param name="torrentUrl">下载地址</param>
        /// <param name="sourceUrl">详情地址</param>
        public GameInfoMain(string sourceUrl, string sourceMd5, string gameName, string gameSize,
            string gameVersion, string gameCovert, string torrentUrl)
        {
            GameName = gameName;
            GameSize = gameSize;
            GameVersion = gameVersion;
            GameCovert = gameCovert;
            TorrentUrl = torrentUrl;
            SourceUrl = sourceUrl;
            SourceMd5 = sourceMd5;
        }

        /// <summary>
        /// 游戏名
        /// </summary>
        [StringLength(GameNameLength)]
        public string GameName { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        [StringLength(GameNameLength)]
        public string ChineseName { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        /// <remarks>
        ///  12.3 GB
        /// </remarks>
        [StringLength(GameVersionLength)]
        public string GameSize { get; set; }

        /// <summary>
        /// 游戏版本
        /// </summary>
        [StringLength(GameVersionLength)]
        public string GameVersion { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [StringLength(GameUrlLength)]
        public string GameCovert { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        [StringLength(GameUrlLength)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 游戏截图列表
        /// </summary>
        public List<string> ScreenCapture { get; set; }

        /// <summary>
        /// 视频Url
        /// </summary>
        [StringLength(GameUrlLength)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// 源Md5 用于检查网页更新
        /// </summary>
        [StringLength(VideoMain.VideoContentFeatureLength)]
        public string SourceMd5 { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        [StringLength(GameUrlLength)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 种子下载地址
        /// </summary>
        [StringLength(GameUrlLength)]
        public string TorrentUrl { get; set; }

        /// <summary>
        /// 磁力链接
        /// </summary>
        [StringLength(GameUrlLength)]
        public string Magnet { get; set; }

        /// <summary>
        /// 详情Id
        /// </summary>
        [StringLength(VideoMain.VideoContentFeatureLength)]
        public string DetailId { get; set; }

        /// <summary>
        /// 用户Hash 
        /// </summary>
        /// <remarks>
        /// 使用DetailId和UserHash 获取stream Url
        /// </remarks>
        [StringLength(UserHashLength)]
        public string UserHash { get; set; }

        /// <summary>
        /// 下载列表
        /// </summary>
        public List<GameInfoWithDownItem> DownList { get; set; }
    }
}
