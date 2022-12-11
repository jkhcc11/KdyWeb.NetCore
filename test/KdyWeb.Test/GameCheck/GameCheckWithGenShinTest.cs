using System.Linq;
using System.Threading.Tasks;
using KdyWeb.Dto.HttpApi;
using KdyWeb.IService.HttpApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.GameCheck
{
    [TestClass]
    public class GameCheckWithGenShinTest : BaseTest<IGameCheckWithGenShinHttpApi>
    {
        private static string cookie = "debug";
        [TestMethod]
        public async Task QueryDailyNote()
        {
            var input = new QueryDailyNoteInput("150371929", "xV8v4Qu54lUKrEYFZkJhB8cuOh9Asafs", "2.23.1", cookie);
            var t = await _service.QueryDailyNoteAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task SignAsync()
        {
            var input = new BBsSignRewardInput("150371929", "z8DRIUjNDT7IT5IZXvrUAxyupA1peND9", "2.34.1",
                cookie);
            var t = await _service.BBsSignRewardAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task QuerySignAsync()
        {
            var input = new QuerySignInfoInput("150371929", "z8DRIUjNDT7IT5IZXvrUAxyupA1peND9", "2.34.1",
                cookie);
            var t = await _service.QuerySignInfoAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task QueryUserBindInfoAsync()
        {
            var input = new QueryUserBindInfoByCookieInput()
            {
                Cookie = cookie,
                Version = "2.34.1"
            };
            var t = await _service.QueryUserBindInfoByCookieAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task GetAllChannels()
        {
            var service = _host.Services.GetService<ILiveTvHttpApi>();

            var t = await service.GetAllChannelsAsync();
            var china = t.Where(a => a.IsChina()).ToList();

            Assert.IsTrue(china.Any());
        }

        [TestMethod]
        public async Task GetAllStreams()
        {
            var service = _host.Services.GetService<ILiveTvHttpApi>();

            var t = await service.GetAllStreamsAsync();
            var valid = t.Where(a => a.IsValid()).ToList();

            Assert.IsTrue(valid.Any());
        }

        [TestMethod]
        public async Task GetChinaValid()
        {
            var service = _host.Services.GetService<ILiveTvHttpApi>();

            var channels = await service.GetAllChannelsAsync();
            var t = await service.GetAllStreamsAsync();

            var china = channels.Where(a => a.IsChina()).ToList();
            var valid = t.Where(a => a.IsValid() &&
                                     china.Any(b => b.ChannelId == a.ChannelId)).ToList();

            Assert.IsTrue(valid.Any());
        }
    }
}
