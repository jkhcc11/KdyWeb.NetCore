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

        [TestMethod]
        public async Task CreateUserAsync()
        {
            var input = new CreateUserInput()
            {
                UserName = "test11",
                UserEmail = "admin@111.com",
                UserNick = "test11",
                UserPwd = "123456"
            };
            var result = await _service.CreateUserAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task CheckUserExitAsync()
        {
            var input = new CheckUserExitInput()
            {
                UserName = "test",
            };
            var result = await _service.CheckUserExitAsync(input);
            Assert.IsTrue(result.IsSuccess == false);
        }
    }
}
