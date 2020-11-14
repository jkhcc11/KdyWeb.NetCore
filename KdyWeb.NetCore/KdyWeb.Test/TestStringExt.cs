using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    [TestClass]
    public class TestStringExt : BaseTest<IConfiguration>
    {
        [TestMethod]
        public void TestDes()
        {

            var key = _service.GetValue<string>(KdyWebServiceConst.DesKey);
            var sourceStr = "123456";

            var testStr = "DD46293BBF4EA83D";
            var testNew = testStr.DesHexToStr(key);

            var hexStr = sourceStr.ToDesHexExt(key);

            var newStr = hexStr.DesHexToStr(key);

            Assert.IsTrue(newStr == sourceStr);
        }
    }
}
