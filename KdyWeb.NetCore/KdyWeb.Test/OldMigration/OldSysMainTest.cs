using System.Threading.Tasks;
using KdyWeb.IService.OldMigration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.OldMigration
{
    [TestClass]
    public class OldSysMainTest : BaseTest<IOldSysMainService>
    {
        [TestMethod]
        public async Task OldToNew()
        {
            var result = await _service.OldToNewMain(1, 12);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task OldToNewUser()
        {
            var result = await _service.OldToNewUser(1, 1);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
