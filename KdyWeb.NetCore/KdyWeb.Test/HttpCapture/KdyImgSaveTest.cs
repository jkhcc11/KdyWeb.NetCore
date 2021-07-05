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
                ImgUrl = "https://tu3.shanzhuo.cc/s/2019/dyg/vod/2019-11/5dcfeabceff4e.jpeg"
                // ImgUrl = "https://i.niupic.com/images/2021/07/01/9mBe.jpg"
            };

            var result = await _service.PostFileByUrl(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
