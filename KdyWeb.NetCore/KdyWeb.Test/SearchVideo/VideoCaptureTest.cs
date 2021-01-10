using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class VideoCaptureTest : BaseTest<IVideoCaptureService>
    {
        [TestMethod]
        public async Task TestCreate()
        {
            var input = new CreateVideoInfoByDetailInput()
            {
                DetailUrl = "http://www.zuidazy3.net/?m=vod-detail-id-100997.html",
                VideoName = "永远的第一名"
            };
            var result = await _service.CreateVideoInfoByDetailAsync(input);
            Assert.IsTrue(result.IsSuccess == false);

            await Task.Delay(2000);

            input = new CreateVideoInfoByDetailInput()
            {
                DetailUrl = "http://www.zuidazy3.net/?m=vod-detail-id-95791.html",
                VideoName = "24小时日本"
            };
            result = await _service.CreateVideoInfoByDetailAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
