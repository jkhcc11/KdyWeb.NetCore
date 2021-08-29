using System;
using System.Threading.Tasks;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class VideoHistoryTest : BaseTest<IVideoHistoryService>
    {

        [TestMethod]
        public async Task TestCreate()
        {
            var input = new CreateVideoHistoryInput()
            {
                EpId = 1348202781462368256
            };
            var result = await _service.CreateVideoHistoryAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestQuery()
        {
            var queryInput = new QueryVideoHistoryInput()
            {
                StartTime = DateTime.Now.Date,
                EndTime = Convert.ToDateTime($"{DateTime.Now:yyyy-MM-dd 23:59:59}"),
                Subtype = Subtype.Movie
            };
            var result = await _service.QueryVideoHistoryAsync(queryInput);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
