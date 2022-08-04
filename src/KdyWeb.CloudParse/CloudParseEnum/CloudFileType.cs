namespace KdyWeb.CloudParse.CloudParseEnum
{
    /// <summary>
    /// 云盘文件类型
    /// </summary>
    public enum CloudFileType
    {
        /// <summary>
        /// 视频
        /// </summary>
        Video = 1,

        /// <summary>
        /// 需要切片才可以播放
        /// </summary>
        /// <remarks>
        /// 网盘待切片转码
        /// </remarks>
        VideoTs = 101,

        /// <summary>
        /// 音频
        /// </summary>
        Audio = 2,

        /// <summary>
        /// 图片
        /// </summary>
        Image = 3,

        /// <summary>
        /// 文件
        /// </summary>
        File = 5,

        /// <summary>
        /// 文件夹
        /// </summary>
        Dir = 10
    }
}
