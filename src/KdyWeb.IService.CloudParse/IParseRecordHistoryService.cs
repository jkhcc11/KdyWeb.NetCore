using System.Collections.Generic;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;

namespace KdyWeb.IService.CloudParse
{
    /// <summary>
    /// 解析记录 服务接口
    /// </summary>
    public interface IParseRecordHistoryService : IKdyService
    {
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<KdyResult<PageList<QueryParseRecordHistoryDto>>> QueryParseRecordHistoryAsync(QueryParseRecordHistoryInput input);

        /// <summary>
        /// 获取访问前N条文件信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetTopFileInfoDto>>> GetTopFileInfoAsync(GetTopFileInfoInput input);

        /// <summary>
        /// 查询时间范围内记录按天汇总
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<QueryRecordDaySumByDateRangeDto>>> QueryRecordDaySumByDateRangeAsync(QueryRecordSumByDateRangeInput input);
    }
}
