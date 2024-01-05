using KdyWeb.IService.HttpApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class DouBanTest : BaseTest<IDouBanHttpApi>
    {

        [TestMethod]
        public async Task Decode()
        {
            var search = await _service.SearchSuggestAsync("潜伏");
            var search1 = await _service.KeyWordSearchAsync("潜伏", 1);

            Assert.IsTrue(search.IsSuccess);
            Assert.IsTrue(search1.IsSuccess);
        }
    }
}
