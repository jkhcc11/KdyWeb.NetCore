using System.Threading.Tasks;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.FileStore;
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
                ImgUrl = "https://img2.doubanio.com/view/celebrity/raw/public/p1378956633.91.jpg"
                // ImgUrl = "https://i.niupic.com/images/2021/07/01/9mBe.jpg"
            };

            var result = await _service.PostFileByUrl(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
