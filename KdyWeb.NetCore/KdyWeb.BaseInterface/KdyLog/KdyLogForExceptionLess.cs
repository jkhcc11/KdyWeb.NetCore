using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Exceptionless;
using Exceptionless.Logging;
using Microsoft.AspNetCore.Http;

namespace KdyWeb.BaseInterface.KdyLog
{
    /// <summary>
    /// 通过ExceptionLess记录日志
    /// </summary>
    public class KdyLogForExceptionLess : IKdyLog
    {
        private static readonly ExceptionlessClient Client = ExceptionlessClient.Default;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KdyLogForExceptionLess(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Info(string info, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(GetSource(), info, LogLevel.Info);
            InitData(client, extInfo, tags).Submit();
        }

        public void Debug(string info, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(GetSource(), info, LogLevel.Debug);
            InitData(client, extInfo, tags).Submit();
        }

        public void Warn(string info, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(GetSource(), info, LogLevel.Warn);
            InitData(client, extInfo, tags).Submit();
        }

        public void Other(string info, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(GetSource(), info, LogLevel.Other);
            InitData(client, extInfo, tags).Submit();
        }

        public void Error(Exception ex, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            var client = ex.ToExceptionless()
                 .AddTags(tags);
            if (extInfo != null && extInfo.Any())
            {
                foreach (var item in extInfo)
                {
                    client.AddObject(item.Value, item.Key, ignoreSerializationErrors: true);
                }
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                client.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            client.Submit();
        }

        public void Trace(string info, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(GetSource(), info, LogLevel.Trace);
            InitData(client, extInfo, tags).Submit();
        }

        /// <summary>
        /// 获取调用源
        /// </summary>
        /// <returns></returns>
        private string GetSource()
        {
            //todo:暂时写死获取的第二个 可能后面有问题
            //当前堆栈信息
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            var sourceName = nameof(Info);
            if (frames != null && frames.Length > 3)
            {
                //方法所属类
                //item.GetMethod().DeclaringType.ToString()
                //方法名
                //item.GetMethod().ToString()
                //方法信息
                var frame = frames[2];
                var methodInfo = frame.GetMethod();
                if (methodInfo != null)
                {
                    var className = "";
                    var classType= methodInfo.DeclaringType;
                    while (true)
                    {
                        if (classType == null)
                        {
                            break;
                        }

                        className = classType.FullName;
                        classType = classType.DeclaringType;
                    }

                    sourceName = $"{className}->{methodInfo}:{frame.GetNativeOffset()}";
                }
            }

            return sourceName;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        public EventBuilder InitData(EventBuilder eventBuilder, Dictionary<string, object> extInfo = null, params string[] tags)
        {
            eventBuilder.AddTags(tags);

            if (extInfo != null && extInfo.Any())
            {
                foreach (var item in extInfo)
                {
                    eventBuilder.AddObject(item.Value, item.Key, ignoreSerializationErrors: true);
                }
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                eventBuilder.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            return eventBuilder;
        }
    }
}
