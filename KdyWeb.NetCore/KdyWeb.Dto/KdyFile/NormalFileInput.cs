using System.Collections.Generic;
using System.Text;
using Exceptionless.Json;

namespace KdyWeb.Dto.KdyFile
{
    /// <summary>
    /// 普通文件上传
    /// </summary>
    public class NormalFileInput : BaseKdyFileInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="baseUrl">站点Url</param>
        /// <param name="nameField">文件标签Name</param>
        /// <param name="jsonRule">提取规则</param>
        /// <param name="fileName">文件名</param>
        public NormalFileInput(string baseUrl, string nameField, string jsonRule, string fileName)
        {
            BaseUrl = baseUrl;
            NameField = nameField;
            JsonRule = jsonRule;
            SetFileName(fileName);
        }

        /// <summary>
        /// 站点Url
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 文件标签Name
        /// </summary>
        public string NameField { get; set; }

        /// <summary>
        /// 网页编码 默认UTF8
        /// </summary>
        [ExceptionlessIgnore]
        public Encoding Charset { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 表单其他字段
        /// </summary>
        public Dictionary<string, string> PostParDic { get; set; }

        /// <summary>
        /// 提取规则
        /// </summary>
        /// <remarks>
        /// 如果是Json返回则使用此规则提取
        /// </remarks>
        public string JsonRule { get; set; }

        /// <summary>
        /// 跨域回调提取规则 可以和JsonRule同时使用 优先级高
        ///<example>如fansAdminImgCallback(。。。。 则此值为:fansAdminImgCallback</example>
        /// </summary>
        public string CallBackRule { get; set; }

        /// <summary>
        /// 有些图床返回的Url不包含Host 需要单独加
        /// </summary>
        public string BaseHost { get; set; }

        /// <summary>
        /// 超时（秒）
        /// </summary>
        public int TimeOut { get; set; } = 3;
    }
}
