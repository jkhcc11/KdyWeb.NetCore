namespace KdyWeb.CloudParse.SelfHost.Models
{

    public class JsonParseDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        public virtual JsonParseStatus Code { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public virtual bool Success => Code == JsonParseStatus.Success;

        /// <summary>
        /// 播放地址
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// 是否多音轨
        /// </summary>
        /// <remarks>
        /// mp4多音轨 可使用potplayer
        /// </remarks>
        public virtual bool IsMultipleAudioTracks { get; set; }

        public static JsonParseDto SetSuccess(string url)
        {
            return SetSuccess(JsonParseStatus.Success, url);
        }

        public static JsonParseDto SetSuccess(JsonParseStatus code, string url)
        {
            return new JsonParseDto()
            {
                Code = code,
                Url = url
            };
        }

        public static JsonParseDto SetFail(string msg)
        {
            return SetSuccess(JsonParseStatus.Fail, msg);
        }

        public static JsonParseDto SetFail(JsonParseStatus code, string msg)
        {
            return new JsonParseDto()
            {
                Code = code,
                Message = msg
            };
        }
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum JsonParseStatus
    {
        Success = 200,
        Fail = 500
    }
}
