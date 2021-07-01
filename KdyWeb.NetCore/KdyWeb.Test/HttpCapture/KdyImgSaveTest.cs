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
                ImgUrl = "https://tu3.shanzhuo.cc/s/2021/xp/vod/2021-04/6083a9ab637a2.jpg"
            };

            var result = await _service.PostFileByUrl(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
