﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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
        /// <param name="source">来源</param>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Info(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags);

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Debug(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags);

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Warn(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags);

        /// <summary>
        /// 其他信息
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="info">信息</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Other(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags);

        /// <summary>
        /// 错误异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="tags">标签</param>
        void Error(Exception ex, Dictionary<string, string> extInfo = null, params string[] tags);
    }
}
