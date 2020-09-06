using System;
using System.Collections.Generic;
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


        public void Info(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(source, info, LogLevel.Info)
                .AddTags(tags);
            if (extInfo != null && extInfo.Any())
            {
                client.SetManualStackingInfo("KdyExtInfo", extInfo);
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                client.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            client.Submit();
        }

        public void Debug(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(source, info, LogLevel.Debug)
                .AddTags(tags);
            if (extInfo != null && extInfo.Any())
            {
                client.SetManualStackingInfo("KdyExtInfo", extInfo);
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                client.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            client.Submit();
        }

        public void Warn(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(source, info, LogLevel.Warn)
                 .AddTags(tags);
            if (extInfo != null && extInfo.Any())
            {
                client.SetManualStackingInfo("KdyExtInfo", extInfo);
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                client.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            client.Submit();
        }

        public void Other(string source, string info, Dictionary<string, string> extInfo = null, params string[] tags)
        {
            var client = Client.CreateLog(source, info, LogLevel.Other)
                 .AddTags(tags);
            if (extInfo != null && extInfo.Any())
            {
                client.SetManualStackingInfo("KdyExtInfo", extInfo);
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                client.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            client.Submit();
        }

        public void Error(Exception ex, Dictionary<string, string> extInfo = null, params string[] tags)
        {
            var client = ex.ToExceptionless()
                 .AddTags(tags);
            if (extInfo != null && extInfo.Any())
            {
                client.SetManualStackingInfo("KdyExtInfo", extInfo);
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                client.AddTags(_httpContextAccessor.HttpContext.TraceIdentifier)
                    .SetHttpContext(_httpContextAccessor.HttpContext);
            }

            client.Submit();
        }

    }
}
