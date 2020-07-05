using System;
using Exceptionless;
using Exceptionless.Logging;

namespace KdyWeb.BaseInterface.KdyLog
{
    /// <summary>
    /// 通过ExceptionLess记录日志
    /// </summary>
    public class KdyLogForExceptionLess : IKdyLog
    {
        private readonly ExceptionlessClient _client;

        public KdyLogForExceptionLess()
        {
            _client = ExceptionlessClient.Default;
        }

        public void Info(string source, string info, params string[] tags)
        {
            _client.CreateLog(source, info, LogLevel.Info)
                .AddTags(tags)
                .Submit();
        }

        public void Debug(string source, string info, params string[] tags)
        {
            _client.CreateLog(source, info, LogLevel.Debug)
                .AddTags(tags)
                .Submit();
        }

        public void Warn(string source, string info, params string[] tags)
        {
            _client.CreateLog(source, info, LogLevel.Warn)
                .AddTags(tags)
                .Submit();
        }

        public void Other(string source, string info, params string[] tags)
        {
            _client.CreateLog(source, info, LogLevel.Other)
                .AddTags(tags)
                .Submit();
        }

        public void Error(Exception ex, params string[] tags)
        {
            ex.ToExceptionless()
                .AddTags(tags)
                .Submit();
        }

    }
}
