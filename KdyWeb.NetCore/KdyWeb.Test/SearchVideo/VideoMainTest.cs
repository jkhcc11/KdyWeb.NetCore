using System.Threading.Tasks;
using KdyWeb.Dto;
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

        [TestMethod]
        public async Task TestQueryVideoMainAsync()
        {
            var input = new QueryVideoMainInput()
            {
                Genres = "动作"
            };

            var result = await _service.QueryVideoMainAsync(input);
            Assert.IsTrue(result.Data.DataCount > 0);
        }


        [TestMethod]
        public async Task TestUpdateValueByFieldAsync()
        {
            var input = new UpdateValueByFieldInput()
            {
                Id = 1326171992466001920,
                //Field = "SourceUrl",
                //Value = "test"
                Field = "IsMatchInfo",
                Value = "true"
            };

            var result = await _service.UpdateValueByFieldAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
