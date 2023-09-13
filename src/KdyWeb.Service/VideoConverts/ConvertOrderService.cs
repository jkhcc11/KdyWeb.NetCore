using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;
using KdyWeb.IService.VideoConverts;
using KdyWeb.Service.Job;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.VideoConverts
{
    /// <summary>
    /// 转码订单 服务实现
    /// </summary>
    public class ConvertOrderService : BaseKdyService, IConvertOrderService
    {
        private readonly IKdyRepository<ConvertOrder, long> _convertOrderRepository;
        private readonly IKdyRepository<VideoConvertTask, long> _videoConvertTaskRepository;

        public ConvertOrderService(IUnitOfWork unitOfWork,
            IKdyRepository<ConvertOrder, long> convertOrderRepository,
            IKdyRepository<VideoConvertTask, long> videoConvertTaskRepository) : base(unitOfWork)
        {
            _convertOrderRepository = convertOrderRepository;
            _videoConvertTaskRepository = videoConvertTaskRepository;
        }
        /// <summary>
        /// 审批订单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> AuditOrderAsync(AuditOrderInput input)
        {
            var dbOrder = await _convertOrderRepository
                .GetQuery()
                .Include(a => a.OrderDetails)
                .FirstOrDefaultAsync(a => a.Id == input.OrderId);
            if (dbOrder == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,未知Id");
            }

            if (dbOrder.ConvertOrderStatus != ConvertOrderStatus.Init)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,状态非初始化");
            }

            var taskInfo = await _videoConvertTaskRepository.GetQuery()
                .Where(a => dbOrder.OrderDetails.Select(b => b.TaskId).Contains(a.Id) &&
                            a.TaskStatus == VideoConvertTaskStatus.Auditing)
                .ToListAsync();
            if (taskInfo.Count != dbOrder.OrderDetails.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,任务状态未知");
            }

            //订单已完成
            dbOrder.SetFinish(input.ActualAmount);

            //任务已完成
            foreach (var item in taskInfo)
            {
                item.SetFinish();
            }

            _videoConvertTaskRepository.Update(taskInfo);
            _convertOrderRepository.Update(dbOrder);

            await UnitOfWork.SaveChangesAsync();

            var recordType = VodManagerRecordType.Audit;
            if (dbOrder.ActualAmount > 50)
            {
                recordType = VodManagerRecordType.AuditMore;
            }
            var jobInput = new CreateVodManagerRecordInput(LoginUserInfo.GetUserId(), recordType)
            {
                BusinessId = dbOrder.Id,
                Remark = $"任务列表：{string.Join(",", dbOrder.OrderDetails.Select(a => a.TaskName))}",
                LoginUserName = LoginUserInfo.UserName
            };
            BackgroundJob.Enqueue<CreateVodManagerRecordJobService>(a => a.ExecuteAsync(jobInput));
            return KdyResult.Success();
        }

        /// <summary>
        /// 驳回订单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> RejectedOrderAsync(RejectedOrderInput input)
        {
            var dbOrder = await _convertOrderRepository
                .GetQuery()
                .Include(a => a.OrderDetails)
                .FirstOrDefaultAsync(a => a.Id == input.OrderId);
            if (dbOrder == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,未知Id");
            }

            if (dbOrder.ConvertOrderStatus != ConvertOrderStatus.Init)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,状态非初始化");
            }

            var taskInfo = await _videoConvertTaskRepository.GetQuery()
                .Where(a => dbOrder.OrderDetails.Select(b => b.TaskId).Contains(a.Id) &&
                            a.TaskStatus == VideoConvertTaskStatus.Auditing)
                .ToListAsync();
            if (taskInfo.Count != dbOrder.OrderDetails.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,任务状态未知");
            }

            //订单作废
            dbOrder.SetInvalid(input.Remark);

            //任务已驳回
            foreach (var item in taskInfo)
            {
                item.SetRejected();
            }

            _videoConvertTaskRepository.Update(taskInfo);
            _convertOrderRepository.Update(dbOrder);

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 查询转码订单列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryOrderListWithAdminDto>>> QueryOrderListWithAdminAsync(QueryOrderListWithAdminInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(ConvertOrder.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var query = _convertOrderRepository.GetQuery();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                //非管理员 只能查看待审核和自己的审核过的单
                //看不到自己创建的单
                query = query.Where(a => (a.ConvertOrderStatus == ConvertOrderStatus.Init ||
                                         a.ModifyUserId == userId) &&
                                         a.CreatedUserId != userId);
            }

            var pageList = await query
                .Include(a => a.OrderDetails)
                .GetDtoPageListAsync<ConvertOrder, QueryOrderListWithAdminDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 查询我的转码订单列表
        /// </summary>
        /// <returns></returns>s
        public async Task<KdyResult<PageList<QueryMeOrderListDto>>> QueryMeOrderListAsync(QueryMeOrderListInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(ConvertOrder.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var pageList = await _convertOrderRepository.GetQuery()
                .Include(a => a.OrderDetails)
                .Where(a => a.CreatedUserId == userId)
                .GetDtoPageListAsync<ConvertOrder, QueryMeOrderListDto>(input);
            return KdyResult.Success(pageList);
        }

    }
}
