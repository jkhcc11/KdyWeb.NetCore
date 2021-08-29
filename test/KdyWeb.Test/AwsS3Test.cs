using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using KdyWeb.Dto;
using KdyWeb.IService;
using KdyWeb.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    [TestClass]
    public class AwsS3Test : BaseTest<IKdyUserService>
    {
        [TestMethod]
        public void GetUserInfoAsync()
        {
            var secretAccessKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";

            var fileUrl = "/test.txt";
            var requestBuilder = new StringBuilder();
            requestBuilder.Append("GET");
            requestBuilder.Append("\n");

            requestBuilder.Append($"{HttpUtility.HtmlEncode(fileUrl)}");
            requestBuilder.Append("\n");

            //para
            requestBuilder.Append(
                "X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIOSFODNN7EXAMPLE%2F20130524%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20130524T000000Z&X-Amz-Expires=86400&X-Amz-SignedHeaders=host");
            requestBuilder.Append("\n");

            //head
            requestBuilder.Append("host:examplebucket.s3.amazonaws.com");
            requestBuilder.Append("\n");

            requestBuilder.Append("\n");
            //sign head
            requestBuilder.Append("host");
            requestBuilder.Append("\n");

            requestBuilder.Append("UNSIGNED-PAYLOAD");

            var stringToSign = GetSha256Hash(requestBuilder.ToString());
            Assert.IsTrue(stringToSign == "3bfa292879f6447bbcda7001decf97f4a54dc650c8942174ae0a9121cf58ad04");

            var signBuilder = new StringBuilder();
            signBuilder.Append("AWS4-HMAC-SHA256");
            signBuilder.Append("\n");
            signBuilder.Append("20130524T000000Z");
            signBuilder.Append("\n");
            signBuilder.Append("20130524/us-east-1/s3/aws4_request");
            signBuilder.Append("\n");
            signBuilder.Append(stringToSign);

            var dateKey = GetHmacSha256Hash("20130524", $"AWS4{secretAccessKey}");
            var dateRegionKey = GetHmacSha256Hash("us-east-1", dateKey);
            var dateRegionServiceKey = GetHmacSha256Hash("s3", dateRegionKey);
            var signingKey = GetHmacSha256Hash("aws4_request", dateRegionServiceKey);
            var signByte = GetHmacSha256Hash(signBuilder.ToString(), signingKey);
            var sign = signByte.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
            Assert.IsTrue(sign == "aeeed9bbccd4d02ee5c0109b86d86835f995330da4c265957d157751f604d404");
        }

        internal string GetSha256Hash(string data)
        {
            var dataByte = Encoding.UTF8.GetBytes(data);
            var hash = SHA256.Create().ComputeHash(dataByte);
            return hash.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        }

        internal byte[] GetHmacSha256Hash(string data, string key)
        {
            var dataByte = Encoding.UTF8.GetBytes(data);
            var keyByte = Encoding.UTF8.GetBytes(key);
            var hmac = new HMACSHA256(keyByte);
            return hmac.ComputeHash(dataByte);
           // return tempByte.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        }

        internal byte[] GetHmacSha256Hash(string data, byte[] key)
        {
            var dataByte = Encoding.UTF8.GetBytes(data);
          //  var keyByte = Encoding.UTF8.GetBytes(key);
            var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(dataByte);
            // return tempByte.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        }

        //internal byte[] GetHmacSha256Hash(byte[] data, byte[] key)
        //{
        //   // var dataByte = Encoding.UTF8.GetBytes(data);
        //  //  var keyByte = Encoding.UTF8.GetBytes(key);
        //    var hmac = new HMACSHA256(key);
        //    return hmac.ComputeHash(data);
        //    //return tempByte.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        //}
    }
}
