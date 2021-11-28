using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    [TestClass]
    public class TestLog : BaseTest<IConfiguration>
    {
        public const string EncryptStingIndicator = "0secret0";

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

        [TestMethod]
        public void TestSiteServer()
        {
            var t = EncryptStringBySecretKey("xxxxx");
            Assert.IsTrue(t.Length > 5);


            var test = DecryptStringBySecretKey(
                "xxxxxx");
            Assert.IsTrue(test.Length>5);

        }

        public string DecryptStringBySecretKey(string inputString)
        {
            return DecryptStringBySecretKey(inputString, "cc106b2a976f64d");
        }

        private string DecryptStringBySecretKey(string inputString, string secretKey)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            inputString = inputString.Replace(EncryptStingIndicator, string.Empty).Replace("0add0", "+").Replace("0equals0", "=").Replace("0and0", "&").Replace("0question0", "?").Replace("0quote0", "'").Replace("0slash0", "/");

            var encryptor = new DesEncryptor
            {
                InputString = inputString,
                DecryptKey = secretKey
            };
            encryptor.DesDecrypt();

            return encryptor.OutString;
        }


        public string EncryptStringBySecretKey(string inputString)
        {
            return EncryptStringBySecretKey(inputString, "cc106b2a976f64d");
        }

        public string EncryptStringBySecretKey(string inputString, string secretKey)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            var encryptor = new DesEncryptor
            {
                InputString = inputString,
                EncryptKey = secretKey
            };
            encryptor.DesEncrypt();

            var retval = encryptor.OutString;
            retval = retval.Replace("+", "0add0").Replace("=", "0equals0").Replace("&", "0and0").Replace("?", "0question0").Replace("'", "0quote0").Replace("/", "0slash0");

            return retval + EncryptStingIndicator;
        }


    }
}
