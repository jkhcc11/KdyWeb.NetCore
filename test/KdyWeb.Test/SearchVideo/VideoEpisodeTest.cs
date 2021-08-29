using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class VideoEpisodeTest : BaseTest<IVideoEpisodeService>
    {

        [TestMethod]
        public async Task TestUpdateNotEnd()
        {
            var ep = new List<EditEpisodeItem>()
            {
                new EditEpisodeItem()
                {
                    EpisodeName = "test",
                    EpisodeUrl = "2222"
                },
                new EditEpisodeItem()
                {
                    EpisodeName = "tttt",
                    EpisodeUrl = "3333"
                }
            };
            var input = new UpdateNotEndVideoInput(1326171992948346880, "test", false, ep);
            var result = await _service.UpdateNotEndVideoAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
