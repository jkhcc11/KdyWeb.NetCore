using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

        /// <summary>
        /// DES加密返回十六进制
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string ToDesHexExt(this string str, string key)
        {
            byte[] rgbIv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(key);
                //rgbIV与rgbKey可以不一样
                //var rgbIv = keys;
                var inputByteArray = Encoding.UTF8.GetBytes(str);
                var desCryptoService = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, desCryptoService.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray().ByteToHexStr();
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// 十六进制 Des解密
        /// </summary>
        /// <param name="str">待解密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DesHexToStr(this string str, string key)
        {
            byte[] rgbIv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(key);
                // var rgbIv = keys;
                var inputByteArray = str.HexStrToByte();
                if (inputByteArray == null || inputByteArray.Length <= 0)
                {
                    //兼容Base64解密
                    inputByteArray = Convert.FromBase64String(str);
                }

                var desCryptoService = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, desCryptoService.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// 16进制字符串转字节流
        /// </summary>
        /// <param name="hexStr">
        /// 十六进制Hex字符串 <br/>
        /// eg: 2C8721D8ECF5A96F49BC26788EAD9EA08CBC133DAA555A9F61008AEF79E30A68
        /// </param>
        /// <returns></returns>
        public static byte[] HexStrToByte(this string hexStr)
        {
            // 两个十六进制代表一个字节  
            var iLen = hexStr.Length;
            if (iLen <= 0 || 0 != iLen % 2)
            {
                return null;
            }

            var dwCount = iLen / 2;
            var pbBuffer = new byte[dwCount];
            for (var i = 0; i < dwCount; i++)
            {
                var tmp1 = hexStr[i * 2] - (hexStr[i * 2] >= (uint)'A' ? (uint)'A' - 10 : '0');
                if (tmp1 >= 16)
                {
                    return null;
                }

                var tmp2 = hexStr[i * 2 + 1] - (hexStr[i * 2 + 1] >= (uint)'A' ? (uint)'A' - 10 : '0');
                if (tmp2 >= 16)
                {
                    return null;
                }

                pbBuffer[i] = (byte)(tmp1 * 16 + tmp2);
            }
            return pbBuffer;
        }

        /// <summary>
        /// Hex流转字符串
        /// </summary>
        /// <param name="vByte">16进制byte流</param>
        /// <returns></returns>
        public static string ByteToHexStr(this byte[] vByte)
        {
            if (vByte == null || vByte.Length < 1)
            {
                return null;
            }

            var sb = new StringBuilder(vByte.Length * 2);
            foreach (var t in vByte)
            {
                var k = (uint)t / 16;
                sb.Append((char)(k + ((k > 9) ? 'A' - 10 : '0')));
                k = (uint)t % 16;
                sb.Append((char)(k + ((k > 9) ? 'A' - 10 : '0')));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 字符串转十六进制
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static string StrToHex(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return BitConverter.ToString(Encoding.UTF8.GetBytes(str)).Replace("-", "");
        }

        /// <summary>
        /// 字符串混淆
        /// </summary>
        /// <param name="str">待加密Str</param>
        /// <param name="randLength">随机加密</param>
        /// <returns></returns>
        public static string ToStrConfuse(this string str, int randLength = 6)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            //类似360kan返回内容混淆
            var n = str.Length / 2;
            var o = str.Substring(0, n);
            var i = str.Substring(n);
            var rand = DateTime.Now.ToSecondTimestamp().ToString().Substring(0, randLength);
            var endStr = o + rand + i;

            //先随机加入字符串 转为16进制 然后逆序 
            endStr = endStr.StrToHex();
            var charArray = endStr.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        #endregion
    }
}
