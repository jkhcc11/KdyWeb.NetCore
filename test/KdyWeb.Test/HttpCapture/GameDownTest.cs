using System.Threading.Tasks;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.GameDown;
using KdyWeb.IService.HttpApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class GameDownTest : BaseTest<IGameDownWithByrutService>
    {
        private const string Cookie = "cf_clearance=3KDNxW54H_u0vBjOp1Me1LzOp2osPtMrbBch46q4_8M-1670646173-0-160;__cf_bm=9x2fFz.6LnQKXpDWXEDtuQFCdmHBJZOqxEMkIfLW07I-1670646174-0-Abvrcnkn0bUJw9bxOCm/tdj8UkdwXn/tBtbb+F/umqb2a5HKKiufUOQvN/t0uLPsW/pyN8Vz+BWG0Q0IhwR+nYYd8/j+4WLkiagbj8lPxjurr6bjEp0jaJ9OL85wdRNCinMcNimgVzYnL0SU/QkmLZ4=";

        [TestMethod]
        public async Task DetailUrl()
        {
            await _service.CreateDownInfoByDetailUrlAsync("https://byrut.org/29309-mini-battle-ground.html"
            , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"
            , Cookie);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task PageTestTask()
        {
            await _service.QueryPageInfoAsync(3
                , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"
                , Cookie);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TorrentTask()
        {
            var result = await _service.ConvertMagnetByByTorrentUrlAsync(
                new ConvertMagnetByByTorrentInput("https://byrut.org/index.php?do=download&id=80920"
                    , Cookie
                    , "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SteamUrlTask()
        {
            var result = await _service.GetSteamStoreUrlByIdAndUserHashAsync(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"
                , Cookie, "27599", "5b0f30660b0befa8b77e21d03d9e4dff0d25a9c2");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SteamInfoTask()
        {
            var steamService = _host.Services.GetService<ISteamWebHttpApi>();
            var result = await steamService.GetGameInfoByStoreUrlAsync("https://store.steampowered.com/app/1269710");
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
