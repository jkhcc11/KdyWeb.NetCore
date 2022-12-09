using System.IO;

namespace KdyWeb.Dto.GameDown
{
    /// <summary>
    /// 根据种子文件转换磁力
    /// </summary>
    public class ConvertMagnetByByTorrentInput
    {
        public ConvertMagnetByByTorrentInput(string torrentUrl, string cookie, string userAgent)
        {
            TorrentUrl = torrentUrl;
            Cookie = cookie;
            UserAgent = userAgent;
        }

        public ConvertMagnetByByTorrentInput(Stream torrentStream, string cookie, string userAgent)
        {
            TorrentStream = torrentStream;
            Cookie = cookie;
            UserAgent = userAgent;
        }

        /// <summary>
        /// 是否为Url
        /// </summary>
        public bool IsUrl { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string TorrentUrl { get; set; }
        
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream TorrentStream { get; set; }

        /// <summary>
        /// cookie
        /// </summary>
        public string Cookie { get; set; }
        
        /// <summary>
        /// UA
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Referer { get; set; }
    }
}
