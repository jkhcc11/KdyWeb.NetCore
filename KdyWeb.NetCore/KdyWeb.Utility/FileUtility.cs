using System;
using System.IO;

namespace KdyWeb.Utility
{
    /// <summary>
    /// 文件操作工具类
    /// </summary>
    public class FileUtility
    {
        /// <summary>
        /// 文件内容追加
        /// </summary>
        /// <param name="path">文件路径 没有则新建</param>
        /// <param name="msg">追加内容</param>
        public static void Append(string path, string msg)
        {
            File.AppendAllText(path, $"写入时间：{DateTime.Now},写入内容：\r\n{msg}");
        }
    }
}
