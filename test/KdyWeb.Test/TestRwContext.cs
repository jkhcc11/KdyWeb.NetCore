using System;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IRepository;
using KdyWeb.IService.SearchVideo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KdyWeb.Test
{
    /// <summary>
    /// 读写测试
    /// </summary>
    /// <remarks>
    ///  基于数据库读写分离 数据一致性由数据库维护 SqlServer使用 发布订阅  Mysql使用主从 等方式
    /// </remarks>
    [TestClass]
    public class TestRwContext : BaseTest<IDouBanInfoService>
    {
        private readonly IKdyRepository<DouBanInfo, int> _douBanInfoRepository;

        public TestRwContext()
        {
            _douBanInfoRepository = _host.Services.GetService<IKdyRepository<DouBanInfo, int>>();
        }


        [TestMethod]
        public async Task TestRollback()
        {
            var writerRepository = _host.Services.GetService<IKdyRepository<DouBanInfo, int>>();
            var uw = _host.Services.GetService<IUnitOfWork>();

            var guid = Guid.NewGuid().ToString("N");
            //写库
            var dbDouBan = new DouBanInfo()
            {
                VideoTitle = guid ,
                VideoImg = "https://www.baidu.com",
                VideoDetailId = "123456"
            };
            await writerRepository.CreateAsync(dbDouBan);
            await uw.SaveChangesAsync();

            var readGet = await _douBanInfoRepository.GetListAsync(a => a.VideoTitle == guid);


            Assert.IsTrue(readGet.Count > 0);
        }
    }
}
