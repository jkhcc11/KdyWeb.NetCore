using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class VideoMainTest : BaseTest<IVideoMainService>
    {

        [TestMethod]
        public async Task TestCreate()
        {
            var input = new CreateForDouBanInfoInput()
            {
                DouBanInfoId = 40,
                EpisodeGroupType = EpisodeGroupType.VideoPlay,
                EpUrl = "//www.baidu.com/play.m3u8"
            };
            var result = await _service.CreateForDouBanInfoAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetDetail()
        {
            var result = await _service.GetVideoDetailAsync(1325383531156869120);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
