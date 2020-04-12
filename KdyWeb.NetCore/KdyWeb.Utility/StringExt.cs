using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KdyWeb.Utility
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExt
    {
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
    }
}
