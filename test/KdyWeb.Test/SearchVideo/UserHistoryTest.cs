using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class UserHistoryTest : BaseTest<IUserHistoryService>
    {

        [TestMethod]
        public async Task TestCreate()
        {
            var input = new CreateUserHistoryInput()
            {
                EpId = 1,
                VodUrl = "/VodPlay/"
            };
            var result = await _service.CreateUserHistoryAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
