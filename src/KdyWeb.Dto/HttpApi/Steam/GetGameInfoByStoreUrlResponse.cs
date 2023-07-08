using System.Collections.Generic;

namespace KdyWeb.Dto.HttpApi.Steam
{
    /// <summary>
    /// 根据商店Url获取游戏信息
    /// </summary>
    public class GetGameInfoByStoreUrlResponse
    {
        /// <summary>
        /// 支持语言
        /// </summary>
        public class SupportLanguageConst
        {
            /// <summary>
            /// 中文UI
            /// </summary>
            public const string ZhUi = "zh_ui";
            /// <summary>
            /// 中文音频
            /// </summary>
            public const string ZhAudio = "zh_audio";
            /// <summary>
            /// 中文字幕
            /// </summary>
            public const string ZhSrt = "zh_srt";

            /// <summary>
            /// 英文UI
            /// </summary>
            public const string EnUi = "en_ui";
            /// <summary>
            /// 英文音频
            /// </summary>
            public const string EnAudio = "en_audio";
            /// <summary>
            /// 英文字幕
            /// </summary>
            public const string EnSrt = "en_srt";
        }

        /// <summary>
        /// 详情Url
        /// </summary>
        public string DetailUrl { get; set; }

        /// <summary>
        /// 游戏名
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CovertUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  预览视频列表
        /// </summary>
        public List<string> MovieUrlList { get; set; }

        /// <summary>
        /// 截图列表
        /// </summary>
        public List<string> ScreenshotList { get; set; }

        /// <summary>
        /// 支持语言
        /// </summary>
        public Dictionary<string, bool> SupperLanguage { get; set; }
    }
}
