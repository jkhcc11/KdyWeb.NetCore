using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// 创建反馈信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateFeedBackInfoAsync(CreateFeedBackInfoInput input)
        {
            var exit = await _kdyRepository.GetQuery()
                .CountAsync(a => a.OriginalUrl == input.OriginalUrl &&
                                 a.FeedBackInfoStatus == FeedBackInfoStatus.Pending &&
                                 a.UserEmail == input.UserEmail);
            if (exit > 0)
            {
                return KdyResult.Error(KdyResultCode.Error, "数据已提交，请等待处理结果");
            }

            var dbFeedBack = input.MapToExt<FeedBackInfo>();
            await _kdyRepository.CreateAsync(dbFeedBack);
            return KdyResult.Success();
        }
    }
}
