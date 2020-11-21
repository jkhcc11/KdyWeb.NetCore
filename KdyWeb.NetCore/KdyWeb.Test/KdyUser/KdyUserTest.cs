using System.Threading.Tasks;
using KdyWeb.Dto;
using KdyWeb.IService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.KdyUser
{
    [TestClass]
    public class KdyUserTest : BaseTest<IKdyUserService>
    {
        [TestMethod]
        public async Task GetUserInfoAsync()
        {
            var input = new GetUserInfoInput()
            {
                UserInfo = "137651076@qq.com"
            };
            var result = await _service.GetUserInfoAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
