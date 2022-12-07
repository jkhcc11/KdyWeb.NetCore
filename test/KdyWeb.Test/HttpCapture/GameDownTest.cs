using System.Threading.Tasks;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.GameDown;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class GameDownTest : BaseTest<IGameDownWithByrutService>
    {
        private const string Cookie = "__cf_bm=qWBgGHK8yOncTgv5HjtFiCW20NpwvbVH1ZK3S89YIHE-1670424388-0-AbjXCvM9a4AaMQK8EQ2KCwv3gj7f93nuCpNYubRvYBGulcnKMECNynytNACmf/c2JiXwZPUqZUHVOpHnxvqDm1WYXhqrNN7qXHJgckOubPqr/s2HV6oA+Cf5RuBExj0Al6HlBzCTbCViWbD+5oTFP80=;cf_clearance=C6CnYAcXYXn6vUxbc8S8RMpbBozV.LaLvQke7EOi69k-1670424398-0-160";

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
    }
}
