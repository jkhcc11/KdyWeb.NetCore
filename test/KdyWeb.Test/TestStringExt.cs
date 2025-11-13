using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore.Internal;
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

        [TestMethod]
        public void TestT()
        {
            var decryptString = "HUVQ5m2xZeJKCScrl4un/TJHwxpjzAs0lBaC2TwBMnwrLY1DaItt7/RtDeTvV786mmgDknuQUDsInQd/Kgwie7gCk2JTI9QCYUr+ACgFZFry2OqHt4BNkoOEP/xA0SAva6jzB50d855th3BpSifI7bxeVAHdlLaSRFVOk5iCUn0/XYhvbFdLNxKfS+EMyJRD4TvuK05X9RO7b+S87G5elOKj2OINPND/pyu9yLxJp2MQjrVSA0X0R1YsD37p0k2icARAbEsrXbUOjfBH32RZ6KsqCR2dpGp2Kk1dMd4kjoto2cFzb5aEMBT4UWByXs668ZacQTG6+13zl0gbOsyfNG1Wcqe2/5fTMDPT4dGw7DRFphnAg3+hPAju3ewXD9mUtB22tHpoNmFOdC0qWJUpneej5rQPqVxebf229U+EEWhLcj0VUuOxRJCvXm8ydbJGWSA7qVm9bCfPjZV9kChH3i9pDXQ+oUO4vyhA9XPNrw63cWQ85PQXM8m1klV5/VEkDGOfHwHO6bNHq5i7ewBphkZ4XgC4iKe2TXcCr1iW1OCBp2WWIonrj1cWCPiLZEaJM3XaEAKq/ueDlAc/jjRrFGQzms2LalFsYI9/pq9FGqByug/o1jvtL5s8cqmlb6dcY6UpzYG9uOpI03T2Vt+ZsjQgtIatmlRR+5dFbkmlXO49JDgzZ54Ic1MlTRnXe83x8UOyQm/G7Go1S1nECxEjL2cWrqGx2yFCZOyKgmliG9s6F9aMyQsqltHU5ZJ/dLjsiduFI67X97v/JnizXsV5qSVeRETQVTIVio9NvYLwsre5YkDvh4bL5PRyTRdrryYIzDeHlhgOo8IFAkYTH/vgEqVBAL+p6nj+r1Z5ctzacrqTTrd7JjJ8O8cyr4GsvcyNE9hlmMofBRJdVGfoLFfvuPJApBlGE+ufGunnomk01bqcIKt9XTSBXLk0f6iWmQzoDphbwIK3QbEomomO4gl11qU3d3XHp60fqEJFZmJhVcGPWOM3VxOALiLnq9NkDMWdnnmQJtfqZDsq0P+7RhSmnUAd0ehFB4jyKH7M5JkBoigOvHRrPkox7GKCHxuxw6YaccQAne50pBvwSs0XsmmbIhmWRMYKGZ/X0+Y1+5EBtoqTcOc2DfqCIdEMXe77cwQJCwyG9aH/cl9/GeS4qI+I4aq411O6YOgn+n1p11WxTXo9WWjGtnh6QO/AQqcUIt0mCUz4xktq5szEKsENhHvs3NnuSVLG0P17QFFf0FqBccl25gD61F3og359ukAjIZ9xYgsZQpqsd8lTTRQAikWM1L5hHR2D5F+rH4hHDlqtl0BDB6IeT9b6dL2GYxsscgqCscYsdYdZV/l8L+KTquc3YqV2HjdqnFiE62eA6NnGjGG1ng7K0cSUtPI0O4DiX3akIi3WbKSirYN0BdA/M3tedCB7aKmy73DiXHxUtpHUYka17xpcYLSLsQISyKQ8TXah9AKdLUWoxW6NV4Un8AyEog==";

            var encoding = Encoding.UTF8;
            byte[] keyByte = encoding.GetBytes("1000390abcdefgabcdefg12");
            byte[] dataBytes = encoding.GetBytes("bohejing0");

            //  var hmac = new HMACMD5(keyByte);

            using (var hmacmd5 = new HMACMD5(keyByte))
            {

                byte[] hashData = hmacmd5.ComputeHash(dataBytes);
                //var aesKey= Convert.ToBase64String(hashData);
                // string t2 = BitConverter.ToString(hashData);
                //  var aesKey = t2.Replace("-", "");

                // var newAesKey = aesKey.PadRight(32, ' ');
                RijndaelManaged aes = new RijndaelManaged();
                // byte[] iv = Encoding.UTF8.GetBytes(aesKey);
                aes.Key = hashData;
                aes.Mode = CipherMode.CBC;
                aes.IV = hashData;
                aes.Padding = PaddingMode.PKCS7;  //


                ICryptoTransform rijndaelDecrypt = aes.CreateDecryptor();
                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] xBuff = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                var str = Encoding.UTF8.GetString(xBuff);
            }

            Assert.IsTrue(false);
        }

        /// <summary>
        /// 冒泡排序
        /// </summary>
        [TestMethod]
        public void TestSort()
        {
            int[] Start(params int[] number)
            {
                if (number == null || number.Any() == false)
                {
                    return new int[] { };
                }

                return Sort(number);
            }

            int[] Sort(int[] number)
            {
                for (var i = 0; i < number.Length; i++)
                {
                    for (var j = i + 1; j < number.Length; j++)
                    {
                        if (number[i] > number[j])
                        {
                            var temp = number[i];
                            number[i] = number[j];
                            number[j] = temp;
                        }
                    }
                }

                return number;
            }

            var testArrays = Start(5, 7, 34, 2, 41, 1, 3);

            Console.WriteLine(string.Join(",", testArrays));
            Assert.IsTrue(testArrays.Any());
        }

        /// <summary>
        /// 文件处理
        /// </summary>
        [TestMethod]
        public void FileHandler()
        {
            //var filePath = "I:\\待处理\\电影合集\\合并";
            var filePath = "I:\\待处理\\处理中\\电影";
            var re = ProcessAllFiles(filePath);

            Assert.IsTrue(re);
        }


        public bool ProcessAllFiles(string rootPath)
        {
            try
            {
                // 获取根目录下的所有子目录
                string[] subDirectories = Directory.GetDirectories(rootPath);

                int processedCount = 0;
                int errorCount = 0;

                foreach (string subDir in subDirectories)
                {
                    try
                    {
                        // 获取目录中的所有mp4文件
                        var mp4Files = Directory.GetFiles(subDir, "*.mp4", SearchOption.AllDirectories);
                        if (mp4Files.Length > 1)
                        {
                            Console.WriteLine($"处理目录失败 '{subDir}': 多文件");
                            continue;
                        }

                        var filePath = mp4Files.First();
                        //当前mp4文件全路径
                        var fileName = Path.GetFileName(filePath);
                        //处理文件名
                        var newFileName = ConvertFileName(filePath);
                        if (string.IsNullOrEmpty(newFileName))
                        {
                            continue;
                        }

                        // 构建新的文件路径（直接放在根目录下）
                        var newFilePath = Path.Combine(rootPath, newFileName);

                        // 移动文件到根目录
                        File.Move(filePath, newFilePath);
                        Console.WriteLine($"成功: {fileName} -> {Path.GetFileName(newFilePath)}");
                        processedCount++;

                        // 如果目录为空，删除空目录
                        if (Directory.GetFiles(subDir, "*", SearchOption.AllDirectories).Length == 0)
                        {
                            Directory.Delete(subDir, true);
                            Console.WriteLine($"删除空目录: {subDir}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"处理目录失败 '{subDir}': {ex.Message}");
                        errorCount++;
                    }
                }

                Console.WriteLine($"\n处理完成！成功: {processedCount} 个文件，失败: {errorCount} 个文件");
                return processedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序执行错误: {ex.Message}");
                return false;
            }
        }

        public string ConvertFileName(string originalName)
        {
            // 使用正则表达式匹配所有需要的部分
            var pattern = @"《(?<chineseName>[^》]+)》(?<year>\d{4}).*?\\(?<englishName>[^/]+?)\.\d{4}";

            // 模式2: 中文名.英文名.年份 (如：饥饿站台.El Hoyo.2020)
            var pattern2 = @"(?<chineseName>[^\\\.]+)\.[^\\\.]*?(?<englishName>[^\\\.]+)\.(?<year>\d{4})";

            // 模式3: 英文名.年份 (如：The.Unseen.2016)
            //var pattern3 = @"(?<chineseName>[^\\\.]+)\.\d+\\(?<englishName>[A-Za-z\.]+)\.(?<year>\d{4})";

            var match = Regex.Match(originalName, pattern);
            if (match.Success == false)
            {
                match = Regex.Match(originalName, pattern2);
                if (match.Success == false)
                {
                   // match = Regex.Match(originalName, pattern3);
                    if (match.Success == false)
                    {
                        //Console.WriteLine($"文件名格式不符合预期: {originalName}");
                        return null;
                    }
                }
            }

            var chineseName = match.Groups["chineseName"].Value;
            var year = match.Groups["year"].Value;
            var englishName = match.Groups["englishName"].Value;

            var language = "英语中字";
            if (originalName.Contains("泰国"))
            {
                language = "泰语中字";
            }
            else if (originalName.Contains("西班牙"))
            {
                language = "西班牙语中字";
            }
            else if (originalName.Contains("日本"))
            {
                language = "日语中字";
            }
            else if (originalName.Contains("韩国"))
            {
                language = "韩语中字";
            }

            return $"{year}{chineseName}.{englishName}.{language}.mp4";
        }
    }
}
