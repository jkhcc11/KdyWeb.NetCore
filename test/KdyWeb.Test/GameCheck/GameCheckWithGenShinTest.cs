using System.Threading.Tasks;
using KdyWeb.Dto.HttpApi;
using KdyWeb.IService.HttpApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.GameCheck
{
    [TestClass]
    public class GameCheckWithGenShinTest : BaseTest<IGameCheckWithGenShinHttpApi>
    {
        private static string cookie = "ltoken=DUjpFKL206XhJ5KZG9CUmI5xXeYebzMawFrYSO4F;ltuid=252040930;cookie_token=MFb8nFOtVrjPrqJauOrWPfc2Rzkx9BbOp18sIFq1;account_id=252040930;";
        [TestMethod]
        public async Task QueryDailyNote()
        {
            var input = new QueryDailyNoteInput("150371929", "xV8v4Qu54lUKrEYFZkJhB8cuOh9Asafs", "2.23.1", cookie);
            var t = await _service.QueryDailyNoteAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task SignAsync()
        {
            var input = new BBsSignRewardInput("150371929", "z8DRIUjNDT7IT5IZXvrUAxyupA1peND9", "2.34.1",
                cookie);
            var t = await _service.BBsSignRewardAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task QuerySignAsync()
        {
            var input = new QuerySignInfoInput("150371929", "z8DRIUjNDT7IT5IZXvrUAxyupA1peND9", "2.34.1",
                cookie);
            var t = await _service.QuerySignInfoAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }

        [TestMethod]
        public async Task QueryUserBindInfoAsync()
        {
            var input = new QueryUserBindInfoByCookieInput()
            {
                Cookie = cookie,
                Version = "2.34.1"
            };
            var t = await _service.QueryUserBindInfoByCookieAsync(input);
            Assert.IsTrue(t.IsSuccess);
        }
    }
}
