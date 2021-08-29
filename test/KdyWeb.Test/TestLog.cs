using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    [TestClass]
    public class TestLog : BaseTest<IConfiguration>
    {

        [TestMethod]
        public void TestBaseLog()
        {
            var testLog = new
            {
                Name = "zs",
                Age = 10
            };
            var testLog1 = new
            {
                Class = "eeee",
                Name = "erererer"
            };

            //todo:这里的如果是ojb是普通实体类时 如果不重写toString()则日志为实体类名称
            _logger.LogInformation("这是TestInfo1,{testLog}", testLog1);
            _logger.LogWarning("这是TestWarn,{0}", testLog);
            _logger.LogTrace("这是TestTrace,{0}", testLog);

            try
            {
                var i = 0;
                var b = 10 / i;
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, "TestErr测试异常");
            }

            Assert.IsTrue(true);
        }
    }
}
