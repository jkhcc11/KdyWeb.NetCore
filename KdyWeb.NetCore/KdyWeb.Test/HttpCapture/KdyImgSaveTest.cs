using System.Threading.Tasks;
using KdyWeb.Dto.KdyImg;
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
            var input = new PostFileByUrlInput()
            {
                ImgUrl = "https://img2.doubanio.com/view/photo/s_ratio_poster/public/p1930234393.jpg"
            };

            var result = await _service.PostFileByUrl(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
