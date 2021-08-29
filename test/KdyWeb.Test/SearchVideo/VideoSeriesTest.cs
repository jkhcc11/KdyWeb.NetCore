using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class VideoSeriesTest : BaseTest<IVideoSeriesService>
    {
        [TestMethod]
        public async Task TestQueryVideoSeriesAsync()
        {
            var input = new QueryVideoSeriesInput()
            {
                KeyWord = "许氏"
            };
            var result = await _service.QueryVideoSeriesAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestCreateVideoSeriesAsync()
        {
            var input = new CreateVideoSeriesInput()
            {
                SeriesName = "系列测试",
                SeriesImg = "//img.zsxcb.net/kdyImg/path/1285547317121912832"
            };
            var result = await _service.CreateVideoSeriesAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
