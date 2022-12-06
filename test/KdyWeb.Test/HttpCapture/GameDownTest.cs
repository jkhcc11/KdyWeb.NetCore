using System.Threading.Tasks;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.GameDown;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class GameDownTest : BaseTest<IGameDownWithByrutService>
    {
        private const string Cookie = "cf_clearance=aaysrsEU9t6MzLjIMUTww5A6zv5eOFLRngAEE4jB.6g-1670325979-0-160;__cf_bm=8Z4NDEuYGH0UBhvvrd9b3YJRfKeSJKTuhn4Wj03imNs-1670325985-0-Ado1D6IfX3RnINqXmwGUGOxsRn1jyVNl63kalkyGVm/ezloVYLMMtEs1bCMUIlHDxYmWDKIu2t8ohcLWHvKfnHv/V4nL1/vkW4+Jb7tPO/wfPcwDKIVY/EWWBBh3QRN6g+FI2Jpt/qpm5OBfWsC+5Sg=";

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
    }
}
