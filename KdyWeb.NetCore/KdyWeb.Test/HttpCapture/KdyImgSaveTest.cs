using System.Threading.Tasks;
using KdyWeb.IService.ImageSave;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class KdyImgSaveTest : BaseTest<IKdyImgSaveService>
    {
        [TestMethod]
        public async Task PostFileByUrl()
        {
            var url = "https://img2.doubanio.com/view/photo/s_ratio_poster/public/p1930234393.jpg";
            var result = await _service.PostFileByUrl(url);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
