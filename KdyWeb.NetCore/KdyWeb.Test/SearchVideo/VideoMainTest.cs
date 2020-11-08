using System.Threading.Tasks;
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
            var result = await _service.CreateVideoInfoAsync();
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
