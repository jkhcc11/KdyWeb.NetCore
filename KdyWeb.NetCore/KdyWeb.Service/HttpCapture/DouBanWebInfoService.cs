using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Entity;
using KdyWeb.IService.HttpCapture;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.HttpCapture
{
    /// <summary>
    /// 豆瓣站点相关 服务接口
    /// </summary>
    public class DouBanWebInfoService : BaseKdyService, IDouBanWebInfoService
    {
        private readonly IKdyRepository<KdyImgSave, long> _kdyImgSaveRepository;
        private readonly IKdyRepository<KdyUser> _kdyUseRepository;

        public DouBanWebInfoService(IKdyRepository<KdyImgSave, long> kdyImgSaveRepository, IKdyRepository<KdyUser> kdyUseRepository)
        {
            _kdyImgSaveRepository = kdyImgSaveRepository;
            _kdyUseRepository = kdyUseRepository;
        }

        /// <summary>
        /// 根据豆瓣Id获取豆瓣信息
        /// </summary>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        public async Task<KdyResult<GetDouBanOut>> GetInfoBySubjectId(string subjectId)
        {
            var tUser = await _kdyUseRepository.GetQuery()
                .ToListAsync();
            var tt = tUser.First();

            var temp = await _kdyImgSaveRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Take(10)
                .ToListAsync();
            var t = temp.First();
            return KdyResult.Success(new GetDouBanOut());
        }
    }
}
