using System.Threading.Tasks;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.Entity.VideoConverts.Enum;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class VideoMainTest : BaseTest<IVideoMainService>
    {

        [TestMethod]
        public async Task TestCreate()
        {
            var input = new CreateForDouBanInfoInput()
            {
                DouBanInfoId = 268,
                EpisodeGroupType = EpisodeGroupType.VideoPlay,
                EpUrl = "//www.baidu.com/play.m3u8"
            };
            var result = await _service.CreateForDouBanInfoAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetDetail()
        {
            var result = await _service.GetVideoDetailAsync(1325383531156869120);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestQueryVideoMainAsync()
        {
            var input = new QueryVideoMainInput()
            {
                Genres = "动作"
            };

            var result = await _service.QueryVideoMainAsync(input);
            Assert.IsTrue(result.Data.DataCount > 0);
        }


        [TestMethod]
        public async Task TestUpdateValueByFieldAsync()
        {
            var input = new UpdateValueByFieldInput()
            {
                Id = 1326171992466001920,
                Field = "SourceUrl",
                Value = "systeminput"
                //Field = "IsMatchInfo",
                //Value = "true",
                //Field = "VideoMainStatus",
                //Value = "Ban"
            };

            var result = await _service.UpdateValueByFieldAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task ModifyVideoMainAsync()
        {
            var input = new ModifyVideoMainInput()
            {
                Id = 1348202780468318208,
                // VideoImg = "tttt",
                Subtype = Subtype.Movie,
                VideoMainStatus = VideoMainStatus.Normal,
                DownUrl = "下载地址$www.baidu.com/1111\r\n下载地址1$www.baidu.com/1111",
                VideoGenres = "ttttt1111"
            };

            var result = await _service.ModifyVideoMainAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetCountInfoBySubtypeAsync()
        {
            var t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2015, 21);
            Assert.IsTrue(t == 2);
            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2015, 25);
            Assert.IsTrue(t == 3);
            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2015, 1);
            Assert.IsTrue(t == 1);
            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2015, 70);
            Assert.IsTrue(t == 3);

            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2019, 21);
            Assert.IsTrue(t == 1);
            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2020, 25);
            Assert.IsTrue(t == 1.5M);
            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2021, 1);
            Assert.IsTrue(t == 0.5M);
            t = VodManagerRecordType.SaveMove.GetSaveInfoCheckoutAmount(2021, 70);
            Assert.IsTrue(t == 3);

            var input = new GetCountInfoBySubtypeInput()
            {
            };

            var result = await _service.GetCountInfoBySubtypeAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task QuerySameVideoByActorAsync()
        {
            var input = new QuerySameVideoByActorInput()
            {
                Actor = "马库斯·格雷厄姆"
            };

            var result = await _service.QuerySameVideoByActorAsync(input);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
