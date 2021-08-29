using System.Threading.Tasks;
using KdyWeb.IService.HttpCapture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class PageSearchConfigTest : BaseTest<IPageSearchConfigService>
    {

        [TestMethod]
        public async Task TestQuery()
        {
            var result = await _service.OneCopyAsync(2);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
