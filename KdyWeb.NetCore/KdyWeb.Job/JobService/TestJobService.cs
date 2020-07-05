using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace KdyWeb.Job.JobService
{
    /// <summary>
    /// 测试 必须继承BackgroundService 说明是后台服务 不会卡死
    /// </summary>
    public class TestJobService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (true)
            //{
            //    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.txt");
            //    await File.AppendAllTextAsync(filePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}", stoppingToken);
            //    await Task.Delay(60000, stoppingToken);
            //}
        }
    }
}
