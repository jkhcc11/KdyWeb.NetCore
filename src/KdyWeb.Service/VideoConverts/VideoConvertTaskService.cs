using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.VideoConverts;
using KdyWeb.Entity.VideoConverts;
using KdyWeb.Entity.VideoConverts.Enum;
using KdyWeb.IService.VideoConverts;
using KdyWeb.Repository;
using KdyWeb.Service.Job;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.VideoConverts
{
    /// <summary>
    /// 视频转码任务 服务实现
    /// </summary>
    public class VideoConvertTaskService : BaseKdyService, IVideoConvertTaskService
    {
        private readonly IKdyRepository<ConvertOrder, long> _convertOrderRepository;
        private readonly IKdyRepository<VideoConvertTask, long> _videoConvertTaskRepository;
        public VideoConvertTaskService(IUnitOfWork unitOfWork,
            IKdyRepository<ConvertOrder, long> convertOrderRepository,
            IKdyRepository<VideoConvertTask, long> videoConvertTaskRepository) : base(unitOfWork)
        {
            _convertOrderRepository = convertOrderRepository;
            _videoConvertTaskRepository = videoConvertTaskRepository;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateTaskAsync(CreateTaskInput input)
        {
            var dbTask = new VideoConvertTask(input.TaskName, input.TaskType, input.GiftPoints,
                input.SourceLinkType, input.SourceLink)
            {
                SourceLinkExt = input.SourceLinkExt,
                TaskRemark = input.TaskRemark
            };
            var any = await _videoConvertTaskRepository
                .GetAsNoTracking()
                .AnyAsync(a => a.TaskName == dbTask.TaskName);
            if (any)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,当前任务名已存在");
            }

            await _videoConvertTaskRepository.CreateAsync(dbTask);
            await UnitOfWork.SaveChangesAsync();

            var jobInput = new CreateVodManagerRecordInput(LoginUserInfo.GetUserId(), VodManagerRecordType.CreateTask)
            {
                BusinessId = dbTask.Id,
                Remark = $"任务名：{dbTask.TaskName}"
            };
            BackgroundJob.Enqueue<CreateVodManagerRecordJobService>(a => a.ExecuteAsync(jobInput));
            return KdyResult.Success();
        }

        /// <summary>
        /// 查询任务列表(admin)
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryConvertTaskWithAdminDto>>> QueryConvertTaskWithAdminAsync(QueryConvertTaskWithAdminInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(VideoConvertTask.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var pageList = await _videoConvertTaskRepository.GetQuery()
                .GetDtoPageListAsync<VideoConvertTask, QueryConvertTaskWithAdminDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 查询任务列表(normal)
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryConvertTaskWithNormalDto>>> QueryConvertTaskWithNormalAsync(QueryConvertTaskWithNormalInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(VideoConvertTask.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var pageList = await _videoConvertTaskRepository.GetQuery()
                .Where(a => a.TaskStatus == VideoConvertTaskStatus.Waiting)
                .GetDtoPageListAsync<VideoConvertTask, QueryConvertTaskWithNormalDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 接任务
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> TakeTaskAsync(List<long> taskIds)
        {
            var dbTask = await _videoConvertTaskRepository
                .GetQuery()
                .Where(a => taskIds.Contains(a.Id))
                .ToListAsync();
            if (dbTask == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "错误id");
            }

            if (dbTask.All(a => a.TaskStatus == VideoConvertTaskStatus.Waiting) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败,任务非正常状态");
            }

            foreach (var item in dbTask)
            {
                item.TakeTask(LoginUserInfo.GetUserId(), LoginUserInfo.UserName);
            }

            _videoConvertTaskRepository.Update(dbTask);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 查询我的任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryMeTaskListDto>>> QueryMeTaskListAsync(QueryMeTaskListInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(VideoConvertTask.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var pageList = await _videoConvertTaskRepository.GetQuery()
                .Where(a => a.TakeUserId == userId)
                .GetDtoPageListAsync<VideoConvertTask, QueryMeTaskListDto>(input);
            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> SubmitTaskAsync(SubmitTaskInput input)
        {
            var userId = LoginUserInfo.GetUserId();

            var taskInfo = await _videoConvertTaskRepository.GetAsNoTracking()
                .Where(a => input.TaskIds.Contains(a.Id))
                .ToListAsync();
            if (input.TaskIds.Count != taskInfo.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "提交失败,数量不一致");
            }

            if (taskInfo.All(a => a.TaskStatus == VideoConvertTaskStatus.Padding) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "提交失败,状态失效");
            }

            if (taskInfo.All(a => a.TakeUserId == userId) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "提交失败,非自己订单");
            }

            //创建Order
            var dbOrder = new ConvertOrder(taskInfo.Sum(a => a.GiftPoints), input.Context)
            {
                OrderRemark = input.Remark,
                OrderDetails = taskInfo.Select(a => new ConvertOrderDetail()
                {
                    TaskId = a.Id,
                    TaskName = a.TaskName
                }).ToList()
            };
            foreach (var taskItem in taskInfo)
            {
                taskItem.SetAuditing();
            }

            _videoConvertTaskRepository.Update(taskInfo);
            await _convertOrderRepository.CreateAsync(dbOrder);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CancelTaskAsync(BatchDeleteForLongKeyInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var taskInfo = await _videoConvertTaskRepository
                .GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            if (input.Ids.Count != taskInfo.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "提交失败,数量不一致");
            }

            if (taskInfo.All(a => a.TaskStatus == VideoConvertTaskStatus.Padding) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "提交失败,状态失效");
            }

            if (LoginUserInfo.IsSuperAdmin == false)
            {
                if (taskInfo.All(a => a.TakeUserId == userId) == false)
                {
                    return KdyResult.Error(KdyResultCode.Error, "提交失败,非自己订单");
                }
            }

            foreach (var item in taskInfo)
            {
                item.CancelTask();
            }

            _videoConvertTaskRepository.Update(taskInfo);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
