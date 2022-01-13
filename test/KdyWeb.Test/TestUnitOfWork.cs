using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    /// <summary>
    /// 工作单元 测试 
    /// </summary>
    [TestClass]
    public class TestUnitOfWork : BaseTest<IUnitOfWork>
    {
        private readonly IKdyRepository<DouBanInfo, int> _douBanInfoRepository;
        private readonly IKdyRepository<FeedBackInfo, int> _feedBackRepository;

        public TestUnitOfWork()
        {
            _douBanInfoRepository = _host.Services.GetService<IKdyRepository<DouBanInfo, int>>();
            _feedBackRepository = _host.Services.GetService<IKdyRepository<FeedBackInfo, int>>();
        }


        [TestMethod]
        public async Task TestRollback()
        {
            //第一步成功
            var dbDouBan = new DouBanInfo()
            {
                VideoTitle = "null",
                VideoImg = "",
                VideoDetailId = ""
            };
            await _douBanInfoRepository.CreateAsync(dbDouBan);
            //保存
            await _service.SaveChangesAsync();

            //第二步异常
            dbDouBan = new DouBanInfo()
            {
                VideoTitle = null,
                VideoImg = "",
                VideoDetailId = ""
            };
            await _douBanInfoRepository.CreateAsync(dbDouBan);

            var url = "http://www.baidu.com";
            var email = "admin@admin.com";
            var feedBackInfo = new FeedBackInfo(UserDemandType.Input, url);

            await _feedBackRepository.CreateAsync(feedBackInfo);

            //统一保存时报错  第二步和第三步回滚  之前在仓储写保存时 每个create都会保存 这样会导致不一致
            //使用Uow统一管理提交
            await Assert.ThrowsExceptionAsync<DbUpdateException>(() => _service.SaveChangesAsync());
        }

        [TestMethod]
        public async Task TestSubmit()
        {
            //第一步正常
            var dbDouBan = new DouBanInfo()
            {
                VideoTitle = "测试标题",
                VideoImg = "https://www.baidu.com",
                VideoDetailId = "123456"
            };
            await _douBanInfoRepository.CreateAsync(dbDouBan);

            var url = "http://www.baidu.com";
            var email = "admin@admin.com";
            //第二部正常
            var feedBackInfo = new FeedBackInfo(UserDemandType.Input, url);

            await _feedBackRepository.CreateAsync(feedBackInfo);

            //统一保存
            var changes = await _service.SaveChangesAsync();

            Assert.IsTrue(changes > 0);
        }

        [TestMethod]
        public async Task TestRw()
        {
            //从库读数据
            var db = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == 4);
            db.VideoTitle = "修改了";

            //主库更新
            _douBanInfoRepository.Update(db);

            //统一保存
            var changes = await _service.SaveChangesAsync();

            Assert.IsTrue(changes > 0);
        }
    }
}
