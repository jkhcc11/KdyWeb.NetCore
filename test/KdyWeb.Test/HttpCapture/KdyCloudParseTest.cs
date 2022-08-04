using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KdyWeb.CloudParse.CloudParseEnum;
using KdyWeb.CloudParse.Input;
using KdyWeb.Dto.HttpCapture.KdyCloudParse;
using KdyWeb.Dto.KdyImg;
using KdyWeb.IService.HttpCapture.KdyCloudParse;
using KdyWeb.IService.ImageSave;
using KdyWeb.Service.HttpCapture.KdyCloudParse;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.HttpCapture
{
    [TestClass]
    public class KdyCloudParseTest : BaseTest<IKdyImgSaveService>
    {
        [TestMethod]
        public async Task TestAli()
        {
            var input = new BaseConfigInput(1, "tyest_123", "111111111");
            IAliYunCloudParseService parseService = new AliYunCloudParseService(input);
            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>()
            {
                InputId = "5fb76286568fc1ffa9a2401891f59ada387d3dfa"
            });

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Count == 3);

            var fileInfo = fileList.Data.First(a => a.FileType != CloudFileType.Dir);
            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<DownTypeExtData>("test_1_id", fileInfo.ResultId));
            Assert.IsTrue(downInfo.IsSuccess);

            //name下载
            downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<DownTypeExtData>("test_1_name", string.Empty)
            {
                FileName = fileInfo.ResultName,
                ExtData = DownTypeExtData.FileName
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
            var input = new BaseConfigInput(1, "tyest_124", "cloud_web_sid=8243a5fd");
            IStCloudParseService parseService = new StCloudParseService(input);
            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>());

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Any());

            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<StDownExtData>("test_1_id", "3187ebef5e0a41a4a39074bbcee966b8")
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
            var input = new BaseConfigInput(1, "tyest_125", "11111");
            ITyPersonCloudParseService parseService = new TyPersonCloudParseService(input);

            var loginInfo = await parseService.GetLoginInfoAsync();
            Assert.IsNotNull(loginInfo);

            var fileList = await parseService.QueryFileAsync(new BaseQueryInput<string>());

            Assert.IsTrue(fileList.IsSuccess && fileList.Data.Any());

            //id下载
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_1_id", "61499213207699173"));
            Assert.IsTrue(downInfo.IsSuccess);
        }

        [TestMethod]
        public async Task TestTyFamily()
        {
            var input = new BaseConfigInput(1, "tyest_126", "66AAA87C717EE0857E15AC1BAFB03182");
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
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_155_id", "9134112647612263")
            {
                ExtData = familyId
            });
            Assert.IsTrue(downInfo.IsSuccess);
        }

        [TestMethod]
        public async Task TestTyCrop()
        {
            var input = new BaseConfigInput(1, "tyest_127", "COOKIE_LOGIN_USER_CORP=976EF1EBED4DC1CFE406FCC7883AE9627E76874A27DA3A1F95C88585B5E60369D7DF278D7F8A5E8C7868F4B2B13433897FF25CDCBA6D23A38809DBCB3F90F48A0D47089075115F02C942F058859FE2DA3D8EFCCC8F129F6D282D25AD51935794;");
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
            var downInfo = await parseService.GetDownUrlAsync(new BaseDownInput<string>("test_111_id", "61440314511034017")
            {
                ExtData = cropId
            });
            Assert.IsTrue(downInfo.IsSuccess);
        }
    }
}
