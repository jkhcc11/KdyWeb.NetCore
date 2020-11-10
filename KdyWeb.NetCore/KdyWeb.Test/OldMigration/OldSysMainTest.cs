using System;
using System.Collections.Generic;
using System.Text;
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
            var result = await _service.OldToNew();
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
