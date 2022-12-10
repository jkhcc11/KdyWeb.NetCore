using System.Collections.Generic;
using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.GameDown;

namespace KdyWeb.Dto.GameDown
{
    /// <summary>
    /// 查询游戏下载列表 Dto
    /// </summary>
    [AutoMap(typeof(GameInfoMain))]
    public class QueryGameDownListWithAdminDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 游戏名
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        /// <remarks>
        ///  12.3 GB
        /// </remarks>
        public string GameSize { get; set; }

        /// <summary>
        /// 游戏版本
        /// </summary>
        public string GameVersion { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string GameCovert { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 游戏截图列表
        /// </summary>
        public List<string> ScreenCapture { get; set; }

        /// <summary>
        /// 视频Url
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// 源Md5 用于检查网页更新
        /// </summary>
        public string SourceMd5 { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 种子下载地址
        /// </summary>
        public string TorrentUrl { get; set; }

        /// <summary>
        /// 磁力链接
        /// </summary>
        public string Magnet { get; set; }

        /// <summary>
        /// Steam商店Url
        /// </summary>
        public string SteamUrl { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public Dictionary<string, string> ExtInfo { get; set; }
    }
}
