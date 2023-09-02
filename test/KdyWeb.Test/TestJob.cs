using System.Threading.Tasks;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.Dto.Job;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Service.Job;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class TestJob : BaseTest<RecurringVideoJobService>
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        private readonly IKdyRedisCache _kdyRedisCache;
        private readonly RecurringVideoJobService recurringVideoJobService;
        private readonly VideoCaptureJobService _videoCaptureJobService;

        public TestJob()
        {
            _kdyRequestClientCommon = _host.Services.GetRequiredService<IKdyRequestClientCommon>();
            _kdyRedisCache = _host.Services.GetRequiredService<IKdyRedisCache>();

            recurringVideoJobService = new RecurringVideoJobService(_kdyRequestClientCommon, _kdyRedisCache);
            _videoCaptureJobService =
                new VideoCaptureJobService(_host.Services.GetRequiredService<IVideoCaptureService>());
        }

        [TestMethod]
        public async Task TestRecurringVideoJob()
        {
            //获取列表放入队列
            var jobInput = new RecurringVideoJobInput()
            {
                BaseHost = "http://www.kuaibozy.com",
                CaptureDetailXpath = "//video/id",
                OriginUrl = "http://www.kuaibozy.com/api.php/provide/vod/from/kbm3u8/at/xml/?ac=videolist&t=&pg=&h=24&ids=&wd=",
                ServiceFullName = "KdyWeb.Service.HttpCapture.ZyPageParseForJsonService"
            };
            await recurringVideoJobService.ExecuteAsync(jobInput);

            //队列处理
            var detailInput = new VideoCaptureJobInput("http://www.kuaibozy.com/detailId/32037", "", jobInput.ServiceFullName);
            await _videoCaptureJobService.ExecuteAsync(detailInput);

            Assert.IsTrue(true);
        }
    }
}
