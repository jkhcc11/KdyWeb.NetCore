using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace KdyWeb.Dto.HttpCapture.PageParse.CmsJson
{
    /// <summary>
    /// Cms通用返回
    /// </summary>
    public class BaseCmsNormalResultOut<TResult>
    where TResult : class
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public virtual string Msg { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public virtual int Page { get; set; }

        /// <summary>
        /// 结果数量
        /// </summary>
        public virtual int Total { get; set; }

        /// <summary>
        /// 结果集
        /// </summary>
        [JsonProperty("list")]
        public IList<TResult> ResultArray { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <remarks>
        ///  肯定有结果集
        /// </remarks>
        public bool IsSuccess => Total > 0 && ResultArray != null && ResultArray.Any();
    }
}
