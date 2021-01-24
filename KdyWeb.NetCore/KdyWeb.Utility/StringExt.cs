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
            return str.Trim();
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

        /// <summary>
        /// 提取名称中的季数信息  
        /// 【生活大爆炸10】 ---> 10  <br/>
        /// 【生活大爆炸第10季】 ---> 10  <br/>
        /// 【生活大爆炸第十季】 ---> 10 
        /// </summary>
        /// <param name="name">需要提取的关键字</param>
        /// <returns>返回阿拉伯数字</returns>
        public static int GetVideoNameNumberExt(this string name)
        {
            var index = 0;
            var reg = new Regex(@"\w*第([0-9,一,二,三,四,五,六,七,八,九,十]{1,2})([季,章,部]{1})", RegexOptions.RightToLeft);
            var match = reg.Match(name);
            if (match.Groups.Count < 2)
            {
                //匹配类似 七宗罪    和   生活大爆炸10 类似
                reg = new Regex(@"\w*([0-9,一,二,三,四,五,六,七,八,九,十]{1,2})", RegexOptions.RightToLeft);
                match = reg.Match(name);
                if (match.Groups.Count < 2)
                {
                    return index;
                }
            }

            var temp = match.Groups[1].Value;
            switch (temp)
            {
                case "一": temp = "1"; break;
                case "二":
                    temp = "2"; break;
                case "三":
                    temp = "3"; break;
                case "四":
                    temp = "4"; break;
                case "五":
                    temp = "5"; break;
                case "六":
                    temp = "6"; break;
                case "七":
                    temp = "7"; break;
                case "八":
                    temp = "8"; break;
                case "九":
                    temp = "9"; break;
                case "十":
                    temp = "10"; break;
            }
            int.TryParse(temp, out index);
            return index;
        }

        /// <summary>
        /// 比较两个关键字是否相等
        /// </summary>
        /// <remarks>
        ///  适用于第三方站点关键字 和 豆瓣搜索结果匹配
        /// </remarks>
        /// <param name="oldKey">第三方站点关键字</param>
        /// <param name="newKey">豆瓣关键字</param>
        public static bool KeyWordCompare(string oldKey, string newKey)
        {
            if (string.IsNullOrEmpty(oldKey) || string.IsNullOrEmpty(newKey))
            {
                return false;
            }

            if (newKey.Length - oldKey.Length > 5 &&
                newKey.StartsWith(oldKey) == false)
            {
                //差距超过2且 非开头 则跳过
                return false;
            }

            string oldName = oldKey.RemoveStrExt(" "), newName = newKey.RemoveStrExt(" ");
            //精确匹配
            if (oldName == newName)
            {
                return true;
            }

            //获取第几季
            var oldEp = oldName.GetVideoNameNumberExt();
            var newEp = newName.GetVideoNameNumberExt();
            if (oldEp == newEp)
            {
                int minLength = 3;

                if (oldName.Length < minLength ||
                    newName.Length < minLength)
                {
                    //小于3字 默认即可
                    return true;
                }

                var oldStart = oldName.Substring(0, minLength);
                var newStart = newName.Substring(0, minLength);
                if (oldStart != newStart)
                {
                    //名称中数字一样 但是开头不一样
                    // 永远的第一名   第一名  这种返回false
                    return false;
                }

                return true;
            }

            if (oldEp == 0 &&
                newEp == 1 &&
                newName.StartsWith(oldName))
            {
                //xxx 和 xxx 第一季
                return true;
            }

            //不匹配
            return false;
        }

        /// <summary>
        /// 文件名转ContentType 若文件名无后缀则是流类型
        /// </summary>
        /// <returns></returns>
        public static string FileNameToContentType(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(FileNameToContentType) + "参数为空");
            }

            if (fileName.ToLower().EndsWith(".jpg") ||
                fileName.ToLower().EndsWith(".png"))
            {
                return "image/jpeg";
            }

            if (fileName.ToLower().EndsWith(".gif"))
            {
                return "image/gif";
            }

            if (fileName.ToLower().EndsWith(".webp"))
            {
                return "image/webp";
            }

            return "application/octet-stream";
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
