namespace KdyWeb.Dto.GameDown
{
    /// <summary>
    /// 根据种子文件转换磁力
    /// </summary>
    public class ConvertMagnetByByTorrentUrlDto
    {
        /// <summary>
        /// magnet:?xt=urn:btih:1CA512A4822EDC7C1B1CE354D7B8D2F84EE11C32&dn=ubuntu-14.10-desktop-amd64.iso
        /// </summary>
        public string MagnetLink { get; set; }

        /// <summary>
        /// "B415C913643E5FF49FE37D304BBB5E6E11AD5101"
        /// </summary>
        public string InfoHash { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}
