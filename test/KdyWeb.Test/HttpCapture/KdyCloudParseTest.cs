using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.CloudParse.CloudParseEnum;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.IService.CloudParse.DiskCloudParse;
using KdyWeb.IService.FileStore;
using KdyWeb.Service.CloudParse.DiskCloudParse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Secp256k1Net;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class KdyCloudParseTest : BaseTest<IKdyImgSaveService>
    {
        [TestMethod]
        public async Task TestAli()
        {
            var input = new BaseConfigInput("test-123", "tyest_123", 11111);
            IAliYunCloudParseService parseService = new AliYunCloudParseService(input);
            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                InputId = "5fb76286568fc1ffa9a2401891f59ada387d3dfa"
            });

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Count == 3);

            var fileInfo = fileList.Data.First(a => a.FileType != CloudFileType.Dir);
            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_1_id", fileInfo.ResultId
                , DownUrlSearchType.FileId));
            Assert.IsTrue(downInfo.IsSuccess);

            //name下载
            downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_1_name", string.Empty, DownUrlSearchType.Name)
            {
                FileName = fileInfo.ResultName,
            });
            Assert.IsTrue(downInfo.IsSuccess);

            //批量改名
            var bathInput = new List<BatchUpdateNameInput>();
            foreach (var item in fileList.Data)
            {
                bathInput.Add(new BatchUpdateNameInput()
                {
                    FileId = item.ResultId,
                    NewName = $"{item.ResultName}_new",
                    OldName = item.ResultName
                });
            }

            var result = await parseService.BatchUpdateNameAsync(bathInput);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestSt()
        {
            var input = new BaseConfigInput("test-123", "tyest_123", 11111);
            IStCloudParseService parseService = new StCloudParseService(input);
            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>());

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Any());

            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<StDownExtData>("test_1_id",
                "3187ebef5e0a41a4a39074bbcee966b8", DownUrlSearchType.FileId)
            {
                ExtData = new StDownExtData()
                {
                    ParentId = "0d74df1984f741c6b874fd3898dafe73",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36"
                }
            });
            Assert.IsTrue(downInfo.IsSuccess);

            var fileInfo = fileList.Data.First(a => a.FileType != CloudFileType.Dir);
            //批量改名
            var bathInput = new List<BatchUpdateNameInput>
            {
                new BatchUpdateNameInput()
                {
                    FileId = fileInfo.ResultId,
                    NewName = $"{fileInfo.ResultName}_new",
                    OldName = fileInfo.ResultName
                }
            };

            var result = await parseService.BatchUpdateNameAsync(bathInput);
            Assert.IsTrue(result.IsSuccess);

            //离线查询
            var cloudList = await parseService.QueryCloudDownloadAsync(new BaseQueryInput<string>());

            Assert.IsTrue(cloudList.IsSuccess && cloudList.Data.Any());


            //离线查询
            var cloudDown = await parseService.AddCloudDownloadAsync("magnet:?xt=urn:btih:369420671477b7617584a6cc18717748091dda6a");
            Assert.IsTrue(cloudDown.IsSuccess);
        }

        [TestMethod]
        public async Task TestTy()
        {
            var input = new BaseConfigInput("test-123", "tyest_123", 11111);
            ITyPersonCloudParseService parseService = new TyPersonCloudParseService(input);

            var loginInfo = await parseService.GetLoginInfoAsync();
            Assert.IsNotNull(loginInfo);

            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>());

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Any());

            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_1_id",
                "61499213207699173", DownUrlSearchType.FileId));
            Assert.IsTrue(downInfo.IsSuccess);
        }

        [TestMethod]
        public async Task TestTyFamily()
        {
            var input = new BaseConfigInput("test-123", "tyest_123", 11111);
            ITyFamilyCloudParseService parseService = new TyFamilyCloudParseService(input);

            var familyList = await parseService.GetFamilyListAsync();
            Assert.IsTrue(familyList.Count > 0);

            var familyId = familyList.First().Value;
            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                ExtData = familyId
            });

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Any());

            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_155_id",
                "9134112647612263", DownUrlSearchType.FileId)
            {
                ExtData = familyId
            });
            Assert.IsTrue(downInfo.IsSuccess);
        }

        [TestMethod]
        public async Task TestTyCrop()
        {
            var input = new BaseConfigInput("test-123", "tyest_123", 11111);
            ITyCropCloudParseService parseService = new TyCropCloudParseService(input);

            var cropList = await parseService.GetCropListAsync();
            Assert.IsTrue(cropList.Count > 0);

            var cropId = cropList.First().Value;
            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                ExtData = cropId
            });

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Any());

            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_111_id",
                "61440314511034017", DownUrlSearchType.FileId)
            {
                ExtData = cropId
            });
            Assert.IsTrue(downInfo.IsSuccess);
        }

        [TestMethod]
        public void TestSecp256k1()
        {
            using (var secp256K1 = new Secp256k1())
            {
                // 创建私钥
                var privateKey = new byte[Secp256k1.PRIVKEY_LENGTH];
                new Random().NextBytes(privateKey);

                // 获取公钥
                var publicKey = new byte[Secp256k1.PUBKEY_LENGTH];
                secp256K1.PublicKeyCreate(publicKey, privateKey);

                // 创建消息
                byte[] message = Encoding.UTF8.GetBytes("Hello, world!");

                var sha256 = SHA256.Create();
                var msgHash = sha256.ComputeHash(message);

                // 使用私钥对消息进行签名
                var signature = new byte[Secp256k1.SIGNATURE_LENGTH];
                secp256K1.Sign(signature, msgHash, privateKey);

                // 使用公钥验证签名
                bool isVerified = secp256K1.Verify(signature, msgHash, publicKey);

                Console.WriteLine($"Signature is verified: {isVerified}");
            }
        }
    }
}
