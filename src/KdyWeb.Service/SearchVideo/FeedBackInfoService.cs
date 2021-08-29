using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 反馈信息及录入 服务实现
    /// </summary>
    public class FeedBackInfoService : BaseKdyService, IFeedBackInfoService
    {
        private readonly IKdyRepository<FeedBackInfo, int> _kdyRepository;

        public FeedBackInfoService(IKdyRepository<FeedBackInfo, int> kdyRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _kdyRepository = kdyRepository;
        }

        /// <summary>
        /// 分页获取反馈信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<GetFeedBackInfoDto>>> GetPageFeedBackInfoAsync(GetFeedBackInfoInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(FeedBackInfo.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };
            var dbPage = await _kdyRepository.GetQuery()
                .GetDtoPageListAsync<FeedBackInfo, GetFeedBackInfoDto>(input);

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
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 变更反馈信息状态
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ChangeFeedBackInfoAsync(ChangeFeedBackInfoInput input)
        {
            var dbList = await _kdyRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            if (dbList.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无数据结果，处理失败");
            }

            foreach (var item in dbList)
            {
                item.FeedBackInfoStatus = input.FeedBackInfoStatus;
                //  _kdyRepository.Update(item);
            }

            _kdyRepository.Update(dbList);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetFeedBackInfoDto>> GetFeedBackInfoAsync(int id)
        {
            var dbModel = await _kdyRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (dbModel == null)
            {
                return KdyResult.Error<GetFeedBackInfoDto>(KdyResultCode.Error, "参数错误");
            }

            return KdyResult.Success(dbModel.MapToExt<GetFeedBackInfoDto>());
        }

        /// <summary>
        /// 批量删除反馈信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchDeleteForIntKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbEp = await _kdyRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _kdyRepository.Delete(dbEp);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success("删除成功");
        }

        /// <summary>
        /// 获取反馈统计信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetCountInfoDto>>> GetCountInfoAsync(GetCountInfoInput input)
        {
            var query = _kdyRepository.GetAsNoTracking()
                .Where(a => a.CreatedTime >= input.StartTime &&
                            a.CreatedTime <= input.EndTime);
            if (input.FeedBackInfoStatus != null)
            {
                query = query.Where(a => a.FeedBackInfoStatus == input.FeedBackInfoStatus.Value);
            }

            var dbCount = await query
                .GroupBy(a => a.DemandType)
                .Select(a => new GetCountInfoDto
                {
                    DemandType = a.Key,
                    Count = a.Count()
                })
                .ToListAsync();

            return KdyResult.Success(dbCount);
        }
    }
}
