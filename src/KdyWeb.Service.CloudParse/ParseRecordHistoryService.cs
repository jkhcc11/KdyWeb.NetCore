using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.IService.CloudParse;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析记录 服务实现
    /// </summary>
    public class ParseRecordHistoryService : BaseKdyService, IParseRecordHistoryService
    {
        private readonly IKdyRepository<ParseRecordHistory, long> _parseRecordHistoryRepository;

        public ParseRecordHistoryService(IUnitOfWork unitOfWork,
            IKdyRepository<ParseRecordHistory, long> parseRecordHistoryRepository) : base(unitOfWork)
        {
            _parseRecordHistoryRepository = parseRecordHistoryRepository;
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryParseRecordHistoryDto>>> QueryParseRecordHistoryAsync(QueryParseRecordHistoryInput input)
        {
            if (input.OrderBy == null ||
                input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new ()
                    {
                        Key = nameof(ParseRecordHistory.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var timeRange = input.GetRange();
            var query = _parseRecordHistoryRepository.GetQuery();
            query = query.Where(a => a.CreatedTime > timeRange.startTime &&
                                     a.CreatedTime < timeRange.endTime);
            if (input.RecordHistoryType.HasValue)
            {
                query = query.Where(a => a.RecordHistoryType == input.RecordHistoryType);
            }

            var userId = LoginUserInfo.GetUserId();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.UserId == userId);
            }

            if (input.SubAccountId.HasValue)
            {
                query = query.Where(a => a.SubAccountId == input.SubAccountId);
            }

            var result = await query.GetDtoPageListAsync<ParseRecordHistory, QueryParseRecordHistoryDto>(input);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 获取访问前N条文件信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetTopFileInfoDto>>> GetTopFileInfoAsync(GetTopFileInfoInput input)
        {
            var timeRange = input.GetRange();
            var query = _parseRecordHistoryRepository.GetQuery();
            query = query.Where(a => a.CreatedTime > timeRange.startTime &&
                                     a.CreatedTime < timeRange.endTime);
            if (input.RecordHistoryType.HasValue)
            {
                query = query.Where(a => a.RecordHistoryType == input.RecordHistoryType);
            }

            var userId = LoginUserInfo.GetUserId();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.UserId == userId);
            }

            if (input.SubAccountId.HasValue)
            {
                query = query.Where(a => a.SubAccountId == input.SubAccountId);
            }

            if (input.Top > 100 ||
                input.Top <= 0)
            {
                input.Top = 10;
            }

            var group = await
                query.GroupBy(a => a.FileIdOrFileName)
                    .Select(a => new
                    {
                        FileIdOrFileName = a.Key,
                        Count = a.LongCount()
                    })
                    .OrderByDescending(a => a.Count)
                    .Take(input.Top)
                    .ToListAsync();
            var data = group.Select(a => new GetTopFileInfoDto()
            {
                Count = a.Count,
                FileIdOrFileName = a.FileIdOrFileName
            }).ToList();
            return KdyResult.Success(data);
        }

        /// <summary>
        /// 查询时间范围内记录按天汇总
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<QueryRecordDaySumByDateRangeDto>>> QueryRecordDaySumByDateRangeAsync(QueryRecordSumByDateRangeInput input)
        {
            var timeRange = input.GetRange();
            var query = _parseRecordHistoryRepository.GetQuery();
            query = query.Where(a => a.CreatedTime > timeRange.startTime &&
                                     a.CreatedTime < timeRange.endTime);
            if (input.RecordHistoryType.HasValue)
            {
                query = query.Where(a => a.RecordHistoryType == input.RecordHistoryType);
            }

            if (input.SubAccountId.HasValue)
            {
                query = query.Where(a => a.SubAccountId == input.SubAccountId);
            }

            var userId = LoginUserInfo.GetUserId();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.UserId == userId);
            }

            var group = await
                query.GroupBy(a => a.CreatedTime.Date)
                    .Select(a => new
                    {
                        Date = a.Key,
                        Count = a.LongCount()
                    })
                    .OrderBy(a => a.Date)
                    .ToListAsync();
            var data = group.Select(a => new QueryRecordDaySumByDateRangeDto()
            {
                Count = a.Count,
                Date = a.Date
            }).ToList();
            return KdyResult.Success(data);
        }
    }
}
