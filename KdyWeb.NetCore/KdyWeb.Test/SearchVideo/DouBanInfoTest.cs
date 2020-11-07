using System.Diagnostics;
using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class DouBanInfoTest : BaseTest<IDouBanInfoService>
    {
        [TestMethod]
        public async Task TestQuery()
        {
            var result = await _service.QueryDouBanInfoAsync(new QueryDouBanInfoInput());
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestCreate()
        {
            var result = await _service.CreateForSubjectIdAsync("35155748");
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetTop()
        {
            var result = await _service.GetTopDouBanInfoAsync();
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetDetail()
        {
            var result = await _service.GetDouBanInfoForIdAsync(14);
            Assert.IsTrue(result.Data.Id > 0);
        }
    }
}
