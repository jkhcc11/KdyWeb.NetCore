using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyUser;
using KdyWeb.Dto.Message;
using KdyWeb.Dto.VerificationCode;
using KdyWeb.IService;
using KdyWeb.IService.Message;
using KdyWeb.IService.VerificationCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.KdyUser
{
    [TestClass]
    public class KdyUserTest : BaseTest<IKdyUserService>
    {
        private readonly ISendEmailService _sendEmailService;

        public KdyUserTest()
        {
            _sendEmailService = KdyBaseServiceProvider.ServiceProvide.GetService<ISendEmailService>();
        }

        //[TestMethod]
        //public async Task GetUserInfoAsync()
        //{
        //    var input = new GetUserInfoInput()
        //    {
        //        UserInfo = "137651076@qq.com"
        //    };
        //    var result = await _service.GetUserInfoAsync(input);
        //    Assert.IsTrue(result.IsSuccess);
        //}

        [TestMethod]
        public async Task CreateUserAsync()
        {
            var input = new CreateUserInput()
            {
                UserName = "test1234",
                UserEmail = "admin2@111.com",
                UserNick = "test11",
                UserPwd = "12345678"
            };
            var result = await _service.CreateUserAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task CheckUserExitAsync()
        {
            var input = new CheckUserExitInput()
            {
                UserName = "test123",
            };
            var result = await _service.CheckUserExitAsync(input);
            Assert.IsTrue(result.IsSuccess == false);
        }

        [TestMethod]
        public async Task ModifyUserPwdAsync()
        {
            var input = new ModifyUserPwdInput()
            {
                NewPwd = "123456"
            };
            var result = await _service.ModifyUserPwdAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task FindUserPwdAsync()
        {
            var input = new FindUserPwdInput()
            {
                // UserId = 1333386925867929600,
                NewPwd = "1234567",
            };
            var result = await _service.FindUserPwdAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task ModifyUserInfoAsync()
        {
            var input = new ModifyUserInfoInput()
            {
                // UserEmail = "11@qq.com",
                UserNick = "22222"
            };
            var result = await _service.ModifyUserInfoAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task SendCode()
        {
            var code = await _sendEmailService.SendEmailCodeAsync(new SendEmailCodeInput()
            {
                CodeType = VerificationCodeType.Register,
                Email = "137651076@qq.com"
            });
            Assert.IsTrue(code.IsSuccess);
        }

        [TestMethod]
        public async Task CheckCode()
        {
            var codeService = KdyBaseServiceProvider.ServiceProvide.GetService<IVerificationCodeService>();

            var code = await codeService.CheckVerificationCodeAsync(VerificationCodeType.Register, "137651076@qq.com",
                "650570");
            Assert.IsTrue(code.IsSuccess);
        }

        [TestMethod]
        public async Task Login()
        {
            var result = await _service.GetLoginTokenAsync(new GetLoginTokenInput()
            {
                UserName = "137651076@qq.com",
                UserPwd = "Aa123456"
            });

            Assert.IsTrue(result.IsSuccess);

            result = await _service.GetLoginTokenAsync(new GetLoginTokenInput()
            {
                UserName = "test111",
                UserPwd = "Aa123456"
            });

            Assert.IsTrue(result.IsSuccess == false);
        }
    }
}
