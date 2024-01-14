using System.Diagnostics;
using System.Threading.Tasks;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class DouBanInfoTest : BaseTest<IDouBanInfoService>
    {
        private readonly IDouBanWebInfoService _douBanWebInfoService;

        public DouBanInfoTest()
        {
            _douBanWebInfoService = _host.Services.GetService<IDouBanWebInfoService>();
        }

        [TestMethod]
        public async Task TestQuery()
        {
            var result = await _service.QueryDouBanInfoAsync(new QueryDouBanInfoInput());
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestCreate()
        {
            var result = await _service.CreateForSubjectIdAsync("35155748");
            Assert.IsTrue(result.IsSuccess);

        }

        [TestMethod]
        public async Task TestGetTop()
        {
            var result = await _service.GetTopDouBanInfoAsync();
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetDetail()
        {
            var result = await _service.GetDouBanInfoForIdAsync(240);
            Assert.IsTrue(result.Data.Id > 0);
        }

        [TestMethod]
        public async Task GetInfoBySubjectIdForPcWeb()
        {
            var result1 = await _douBanWebInfoService.GetInfoBySubjectId("1898121");
            Assert.IsTrue(result1.IsSuccess);

            var result = await _douBanWebInfoService.GetInfoBySubjectIdForPcWeb("20452294");
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetDouBanInfoByKeyWord()
        {
            var result = await _douBanWebInfoService.GetDouBanInfoByKeyWordAsync("永远的第一名");
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void TestKeyWordCompare()
        {
            string oldStr = "我的一天",
                newStr = "我的一天 第2季";
            var r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);

            oldStr = "我的一天";
            newStr = "我的一天 第1季";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r);

            oldStr = "神盾局特工 第2季";
            newStr = "神盾局特工第二季";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r);

            oldStr = "神盾局特工 第2季";
            newStr = "神盾局特工 第二季";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r);

            oldStr = "神盾局特工2";
            newStr = "神盾局特工 第二季";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r);

            oldStr = "神盾局特工2";
            newStr = "神盾局特工 第三季";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);

            oldStr = "十宗罪";
            newStr = "七宗罪";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);

            oldStr = "十宗罪";
            newStr = "十宗罪2";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);

            oldStr = "铁胆英雄国语";
            newStr = "铁胆英雄";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r);

            oldStr = "铁胆英雄粤语";
            newStr = "铁胆英雄";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r);

            //
            oldStr = "永远的第一名";
            newStr = "第一名";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);

            //
            oldStr = "永远的第一名";
            newStr = "永远之永恒 第一章";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);

            //有数字的名称 必须一致才可以 
            oldStr = "十宗罪";
            newStr = "十宗罪1";
            r = StringExt.KeyWordCompare(oldStr, newStr);
            Assert.IsTrue(r == false);
        }
    }
}
