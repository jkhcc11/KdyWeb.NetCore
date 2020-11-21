using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class UserSubscribeTest:BaseTest<IUserSubscribeService>
    {
        [TestMethod]
        public async Task TestQuery()
        {
            var input = new QueryUserSubscribeInput();
            var result = await _service.QueryUserSubscribeAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
