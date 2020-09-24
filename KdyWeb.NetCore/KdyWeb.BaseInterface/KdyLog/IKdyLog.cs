using System;
using System.Collections.Generic;

namespace KdyWeb.BaseInterface.KdyLog
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface IKdyLog
    {
        /// <summary>
        /// 一般信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Info(string info, Dictionary<string, object> extInfo = null, params string[] tags);

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Debug(string info, Dictionary<string, object> extInfo = null, params string[] tags);

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Warn(string info, Dictionary<string, object> extInfo = null, params string[] tags);

        /// <summary>
        /// 其他信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Other(string info, Dictionary<string, object> extInfo = null, params string[] tags);

        /// <summary>
        /// 错误异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Error(Exception ex, Dictionary<string, object> extInfo = null, params string[] tags);

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Trace(string info, Dictionary<string, object> extInfo = null, params string[] tags);
    }
}
