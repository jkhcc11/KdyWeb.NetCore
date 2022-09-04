﻿using System.Collections.Generic;
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
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 反馈信息及录入 服务实现
    /// </summary>
    public class FeedBackInfoService : BaseKdyService, IFeedBackInfoService
    {
        private readonly IKdyRepository<FeedBackInfo, int> _kdyRepository;
        private readonly IDouBanInfoService _douBanInfoService;

        public FeedBackInfoService(IKdyRepository<FeedBackInfo, int> kdyRepository, IUnitOfWork unitOfWork,
            IDouBanInfoService douBanInfoService) :
            base(unitOfWork)
        {
            _kdyRepository = kdyRepository;
            _douBanInfoService = douBanInfoService;
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

            var query = _kdyRepository.GetQuery();
            if (LoginUserInfo.IsLogin &&
                LoginUserInfo.IsSuperAdmin == false)
            {
                //非管理员登录后只能看自己
                query = query.Where(a => a.CreatedUserId == LoginUserInfo.GetUserId());
            }

            var dbPage = await query
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
                                 a.CreatedUserId == LoginUserInfo.GetUserId());
            if (exit > 0)
            {
                return KdyResult.Error(KdyResultCode.Error, "数据已提交，请等待处理结果");
            }

            var dbFeedBack = input.MapToExt<FeedBackInfo>();
            dbFeedBack.UserEmail = LoginUserInfo.UserEmail;
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

        /// <summary>
        /// 创建求片反馈
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateFeedBackInfoWithHelpAsync(CreateFeedBackInfoWithHelpInput input)
        {
            if (input.OriginalUrl.Contains("douban") == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "仅支持豆瓣链接");
            }
            var tempUrl = input.OriginalUrl;
            if (input.OriginalUrl.Contains("?"))
            {
                tempUrl = input.OriginalUrl.Split('?').First();
            }

            //获取豆瓣信息
            var subjectId = tempUrl.GetNumber();
            var result = await _douBanInfoService.CreateForSubjectIdAsync(subjectId);
            if (result.IsSuccess == false)
            {
                return KdyResult.Error(result.Code, result.Msg);
            }

            var url = $"//movie.douban.com/subject/{subjectId}/";
            //是否已存在
            var exit = await _kdyRepository.GetQuery().CountAsync(a => a.OriginalUrl.Contains(subjectId));
            if (exit > 0)
            {
                return KdyResult.Error(KdyResultCode.Error, "该链接已反馈,请勿重复反馈");
            }

            var dbFeedBack = new FeedBackInfo(UserDemandType.Input, url)
            {
                VideoName = result.Data.VideoTitle,
                Remark = input.Remark,
                UserEmail = LoginUserInfo.UserEmail
            };
            await _kdyRepository.CreateAsync(dbFeedBack);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
