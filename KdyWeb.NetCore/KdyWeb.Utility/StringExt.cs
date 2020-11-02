using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KdyWeb.Utility
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// 为空校验
        /// </summary>
        /// <returns></returns>
        public static bool IsEmptyExt(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Html提取时移除多余字符
        /// </summary>
        /// <returns></returns>
        public static string InnerHtmlHandler(this string str)
        {
            if (str.IsEmptyExt())
            {
                return string.Empty;
            }

            return str.Replace("<![CDATA[", "")
                .Replace("]]>", "")
                .Trim();
        }

        /// <summary>
        /// 移除字符串中的给定字符
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <param name="removeArray">需要移除的字符</param>
        /// <returns></returns>
        public static string RemoveStrExt(this string str, params string[] removeArray)
        {
            if (str.IsEmptyExt() || removeArray == null)
            {
                return str;
            }

            foreach (var item in removeArray)
            {
                str = str.Replace(item, "");
            }
            return str;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns>
        public static string GetStrMathExt(this string str, string s, string e)
        {
            //后面是模式 multiline表示多行 singleline单行
            var rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))",
                RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// Unix时间戳格式 10位
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="length">默认返回10位</param>
        /// <returns>Unix时间戳格式</returns>
        public static long GetUnixExt(DateTime? time = null, int length = 10)
        {
            var resTime = DateTime.Now;
            if (time != null)
            {
                resTime = time.Value;
            }

            var startTime = DateTime.Now.ToUniversalTime();
            switch (length)
            {
                case 13:
                    {
                        //毫秒
                        return (resTime.Ticks - startTime.Ticks) / 10000;
                    }
                default:
                    {
                        //秒
                        return (resTime.Ticks - startTime.Ticks) / 10000000;
                    }
            }
        }

        /// <summary>
        /// 转为Url编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">编码格式 默认utf-8</param>
        /// <returns></returns>
        public static string ToUrlCodeExt(this string str, string encoding = "")
        {
            str = HttpUtility.UrlEncode(str, string.IsNullOrEmpty(encoding) ? Encoding.UTF8 : Encoding.Default);
            return str;
        }

        /// <summary>
        /// Url编码转正常
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlCodeToStrExt(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return HttpUtility.UrlDecode(str);
        }

        #region 加密相关
        /// <summary>
        /// Md5扩展
        /// </summary>
        /// <param name="strSource">待加密字符串</param>
        /// <param name="solt">盐</param>
        /// <returns></returns>
        public static string Md5Ext(this string strSource, string solt = "c1f9b87617d3dc47de27548a9d576702")
        {
            if (string.IsNullOrEmpty(strSource))
                return string.Empty;
            strSource += solt;
            var md5 = new MD5CryptoServiceProvider();
            var data = Encoding.Default.GetBytes(strSource); //将字符编码为一个字节序列
            var md5Data = md5.ComputeHash(data); //计算data字节数组的哈希值
            md5.Clear(); //释放资源
            return md5Data.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0'));
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string Sha1Ext(this string strSource)
        {
            if (string.IsNullOrEmpty(strSource))
                return string.Empty;
            var md5 = new SHA1CryptoServiceProvider();
            var data = Encoding.Default.GetBytes(strSource); //将字符编码为一个字节序列
            var md5Data = md5.ComputeHash(data); //计算data字节数组的哈希值
            md5.Clear(); //释放资源
            return md5Data.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0'));
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="encode">编码方式</param>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static string ToBase64Ext(this string source, Encoding encode)
        {
            string ec;
            byte[] bytes = encode.GetBytes(source);
            try
            {
                ec = Convert.ToBase64String(bytes);
            }
            catch
            {
                ec = source;
            }
            return ec;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="encode">编码方式，必须和编码方式一致</param>
        /// <param name="result">Base64字符串</param>
        /// <returns></returns>
        public static string Base64ToStrExt(this string result, Encoding encode)
        {
            string decode;
            try
            {
                byte[] bytes = Convert.FromBase64String(result);
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        #endregion
    }
}
