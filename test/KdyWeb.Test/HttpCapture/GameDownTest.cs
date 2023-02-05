using System.Threading.Tasks;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.HttpApi.Bilibili;
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
        private const string Cookie = "cf_clearance=DYgXdR3bhLUhuQ.UO5OZ7gxRlFEUYbPCMe_AN_ktDHc-1670509783-0-250;__cf_bm=8zBICHRH_vhX6DZjfA2yABWbm_s4aNTs.nf8.pAWAOs-1670513690-0-AdnilgawQK7TfH/7OmhGGcFbRaMwsShbX/upqLvYh63PQXis/bHevMMv0HZl78UBhJn8imBICaLj7foiuaC8HzL1VO+K0cI812s1Tz4HxVCt6J/sekrG+X9Jz3K8pSYCSNgwdJSqMFrO/MOhGz0y2/8=";

        [TestMethod]
        public async Task DetailUrl()
        {
            await _service.CreateDownInfoByDetailUrlAsync("https://byrut.org/14330-evil-west.html"
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
                new ConvertMagnetByByTorrentInput("https://byrut.org/index.php?do=download&id=76987"
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

        [TestMethod]
        public async Task BTest()
        {
            var steamService = _host.Services.GetService<IBilibiliHttpApi>();
            var result = await steamService.GetVideoInfoByDetailUrlAsync(new GetVideoInfoByDetailUrlRequest()
            {
                DetailUrl = "https://www.bilibili.com/video/BV13A411Q7gS/?spm_id_from=333.337.search-card.all.click"
            });
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
