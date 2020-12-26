using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.IService;
using KdyWeb.IService.HttpCapture;
using KdyWeb.PageParse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class NormalPageParseServiceTest : BaseTest<IKdyUserService>
    {
       // private readonly IPageParseService<NormalPageParseOut, NormalPageParseInput> _pageParseService;

        public NormalPageParseServiceTest()
        {

          //  _zyPageParseService = _host.Services.GetService<IZyPageParseService>();
        }

        [TestMethod]
        public async Task GetResultAsync()
        {
            var normal = "KdyWeb.Service.HttpCapture.NormalPageParseService";
            var pageParseService =
                _host.Services.GetServices<IPageParseService<NormalPageParseOut, NormalPageParseInput>>();
            var normalService = pageParseService.First(a => a.GetType().FullName == normal);

            var result = await normalService.GetResultAsync(new NormalPageParseInput()
            {
                KeyWord = "从结婚开始恋爱",
                ConfigId = 1
            });

            Assert.IsTrue(result.IsSuccess);
            var result2 = await normalService.GetResultAsync(new NormalPageParseInput()
            {
                Detail = "https://www.yst3.com/guochanju/47646.html",
                ConfigId = 1
            });
            Assert.IsTrue(result2.IsSuccess);
        }

        [TestMethod]
        public async Task GetZyResultAsync()
        {
            var zy = "KdyWeb.Service.HttpCapture.ZyPageParseService";
            var pageParseService =
                _host.Services.GetServices<IPageParseService<NormalPageParseOut, NormalPageParseInput>>();
            var zyService = pageParseService.First(a => a.GetType().FullName == zy);


            var result = await zyService.GetResultAsync(new NormalPageParseInput()
            {
                KeyWord = "从结婚开始恋爱",
                ConfigId = 2
            });

            Assert.IsTrue(result.IsSuccess);
            var result2 = await zyService.GetResultAsync(new NormalPageParseInput()
            {
                Detail = "http://www.zuidazy3.net/?m=vod-detail-id-98594.html",
                ConfigId = 2
            });
            Assert.IsTrue(result2.IsSuccess);
        }
    }
}
