using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test.SearchVideo
{
    [TestClass]
    public class FeedBackInfoTest : BaseTest<IFeedBackInfoService>
    {
        [TestMethod]
        public async Task TestQuery()
        {
            var result = await _service.GetPageFeedBackInfoAsync(new GetFeedBackInfoInput());
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestCreate()
        {
            var result = await _service.CreateFeedBackInfoAsync(new CreateFeedBackInfoInput()
            {
                DemandType = UserDemandType.Feedback,
                OriginalUrl = "http://test.test.com",
                Remark = "备注",
                //UserEmail = "admin@admin.com",
                VideoName = "名称"
            });
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestGetFeedBackInfo()
        {
            var result = await _service.GetFeedBackInfoAsync(9);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestChangeFeedBackInfo()
        {
            var result = await _service.ChangeFeedBackInfoAsync(new ChangeFeedBackInfoInput()
            {
                Ids = new[] { 9 },
                FeedBackInfoStatus = FeedBackInfoStatus.Processing
            });
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task BatchDeleteAsync()
        {
            var result = await _service.BatchDeleteAsync(new BatchDeleteForIntKeyInput()
            {
               Ids = new List<int>()
               {
                   1
               }
            });
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetCountInfoAsync()
        {
            var result = await _service.GetCountInfoAsync(new GetCountInfoInput());
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task CreateFeedBackInfoWithHelpAsync()
        {
            var result = await _service.CreateFeedBackInfoWithHelpAsync(new CreateFeedBackInfoWithHelpInput()
            {
                OriginalUrl = "https://movie.douban.com/subject/35068653/",
                Remark = "中文"
            });
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
