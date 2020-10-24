using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 反馈信息及录入 服务实现
    /// </summary>
    public class FeedBackInfoService : BaseKdyService, IFeedBackInfoService
    {
        private readonly IKdyRepository<FeedBackInfo, int> _kdyRepository;

        public FeedBackInfoService(IKdyRepository<FeedBackInfo, int> kdyRepository)
        {
            _kdyRepository = kdyRepository;
        }

        /// <summary>
        /// 分页获取反馈信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<GetFeedBackInfoDto>>> GetPageFeedBackInfoAsync(GetFeedBackInfoInput input)
        {
            var dbPage = await _kdyRepository.GetDtoPageListAsync<GetFeedBackInfoDto>(input.Page, input.PageSize,
                a => a.DemandType == input.UserDemandType);

            return KdyResult.Success(dbPage);
        }
    }
}
