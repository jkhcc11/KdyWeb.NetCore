using System.Threading.Tasks;
using KdyWeb.IService.HttpApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.GameCheck
{
    [TestClass]
    public class GameCheckWithGenShinTest : BaseTest<IGameCheckWithGenShinHttpApi>
    {
        [TestMethod]
        public async Task PostFileByUrl()
        {
            var t = await _service.QueryDailyNote("150371929");
            Assert.IsTrue(t.IsSuccess);
        }
    }
}
