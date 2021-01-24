using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.HttpCapture;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 豆瓣信息 服务实现
    /// </summary>
    public class DouBanInfoService : BaseKdyService, IDouBanInfoService
    {
        private readonly IDouBanWebInfoService _douBanWebInfoService;
        private readonly IKdyRepository<DouBanInfo, int> _douBanInfoRepository;
        private readonly IKdyImgSaveService _kdyImgSaveService;

        public DouBanInfoService(IDouBanWebInfoService douBanWebInfoService, IKdyRepository<DouBanInfo, int> douBanInfoRepository,
            IKdyImgSaveService kdyImgSaveService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _douBanWebInfoService = douBanWebInfoService;
            _douBanInfoRepository = douBanInfoRepository;
            _kdyImgSaveService = kdyImgSaveService;
        }

        /// <summary>
        /// 创建豆瓣信息
        /// </summary>
        /// <param name="subjectId">豆瓣Id</param>
        /// <returns></returns>
        public async Task<KdyResult<CreateForSubjectIdDto>> CreateForSubjectIdAsync(string subjectId)
        {
            CreateForSubjectIdDto result;
            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.VideoDetailId == subjectId);
            if (dbDouBanInfo != null)
            {
                //存在
                result = dbDouBanInfo.MapToExt<CreateForSubjectIdDto>();
                return KdyResult.Success(result);
            }

            //请求豆瓣
            var douBanWebResult = await _douBanWebInfoService.GetInfoBySubjectIdForPcWeb(subjectId);
            if (douBanWebResult.IsSuccess == false)
            {
                return KdyResult.Error<CreateForSubjectIdDto>(KdyResultCode.Error, douBanWebResult.Msg);
            }

            //上传图片
            var imgResult = await _kdyImgSaveService.PostFileByUrl(douBanWebResult.Data.Pic);
            if (imgResult.IsSuccess)
            {
                //上传成功时
                douBanWebResult.Data.Pic = $"{imgResult.Data}";
            }

            KdyLog.Trace($"豆瓣Id:{subjectId}图片上传返回:{imgResult.Msg}", new Dictionary<string, object>()
            {
                {"subjectId",subjectId},
                {"ImgResult",imgResult}
            });

            //保存数据库
            dbDouBanInfo = douBanWebResult.Data.MapToExt<DouBanInfo>();
            await _douBanInfoRepository.CreateAsync(dbDouBanInfo);
            await UnitOfWork.SaveChangesAsync();

            result = dbDouBanInfo.MapToExt<CreateForSubjectIdDto>();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 获取最新豆瓣信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetTop50DouBanInfoDto>>> GetTopDouBanInfoAsync(int topNumber = 50)
        {
            var dbList = await _douBanInfoRepository.GetQuery()
                .OrderByDescending(a => a.CreatedTime)
                .Take(topNumber)
                .ToListAsync();
            var list = dbList.MapToExt<List<GetTop50DouBanInfoDto>>();
            return KdyResult.Success(list);
        }

        /// <summary>
        /// 查询豆瓣信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryDouBanInfoDto>>> QueryDouBanInfoAsync(QueryDouBanInfoInput input)
        {
            input.OrderBy ??= new List<KdyEfOrderConditions>()
            {
                new KdyEfOrderConditions()
                {
                    Key = nameof(DouBanInfo.CreatedTime),
                    OrderBy = KdyEfOrderBy.Desc
                }
            };

            var pageList = await _douBanInfoRepository.GetQuery()
                .GetDtoPageListAsync<DouBanInfo, QueryDouBanInfoDto>(input);

            return KdyResult.Success(pageList);
        }

        /// <summary>
        /// 获取豆瓣信息
        /// </summary>
        /// <param name="douBanInfoId">豆瓣信息Id</param>
        /// <returns></returns>
        public async Task<KdyResult<GetDouBanInfoForIdDto>> GetDouBanInfoForIdAsync(int douBanInfoId)
        {
            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == douBanInfoId);
            if (dbDouBanInfo == null)
            {
                return KdyResult.Error<GetDouBanInfoForIdDto>(KdyResultCode.Error, "Id错误");
            }

            var result = dbDouBanInfo.MapToExt<GetDouBanInfoForIdDto>();
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 变更豆瓣信息状态
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ChangeDouBanInfoStatusAsync(ChangeDouBanInfoStatusInput input)
        {
            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == input.DouBanInfoId);
            if (dbDouBanInfo == null)
            {
                return KdyResult.Error<GetDouBanInfoForIdDto>(KdyResultCode.Error, "Id错误");
            }

            if (dbDouBanInfo.DouBanInfoStatus == DouBanInfoStatus.SearchEnd)
            {
                return KdyResult.Error<GetDouBanInfoForIdDto>(KdyResultCode.Error, "状态已完成，不能更改");
            }

            dbDouBanInfo.DouBanInfoStatus = input.DouBanInfoStatus;
            _douBanInfoRepository.Update(dbDouBanInfo);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        /// <summary>
        /// 根据关键字创建豆瓣信息
        /// </summary>
        /// <remarks>
        ///  若未匹配则直接返回失败
        /// </remarks>
        /// <param name="keyWord">关键字</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public async Task<KdyResult<CreateForSubjectIdDto>> CreateForKeyWordAsync(string keyWord, int year)
        {
            //避免两次请求搜索太快
            await Task.Delay(1800);

            //如果名称相等则直接匹配详情
            //不相等则 先匹配名称和 对应的季数
            var douBanInfo = await _douBanWebInfoService.GetDouBanInfoByKeyWordAsync(keyWord);
            if (douBanInfo.IsSuccess == false)
            {
                return KdyResult.Error<CreateForSubjectIdDto>(douBanInfo.Code, douBanInfo.Msg);
            }

            //避免搜索和详情请求太快
            await Task.Delay(1800);

            var result = KdyResult.Error<CreateForSubjectIdDto>(KdyResultCode.Error, $"匹配豆瓣信息失败，未找到匹配01。{keyWord}");
            foreach (var douBanItem in douBanInfo.Data)
            {
                await Task.Delay(1500);

                var oldKey = keyWord.RemoveStrExt(" ");
                var douBanName = douBanItem.ResultName.RemoveStrExt(" ");
                result = await CreateForSubjectIdAsync(douBanItem.DouBanSubjectId);

                if (result.Data.VideoYear != year)
                {
                    result = KdyResult.Error<CreateForSubjectIdDto>(KdyResultCode.Error, $"匹配豆瓣信息失败，年份不一致。{keyWord}");
                    continue;
                }

                //年份和名称都一样
                if (oldKey == douBanName)
                {
                    break;
                }

                var same = StringExt.KeyWordCompare(oldKey, douBanName);
                if (same)
                {
                    //名称季数都一样
                    break;
                }

                if (string.IsNullOrEmpty(result.Data.Aka) == false &&
                    result.Data.Aka.Contains(oldKey))
                {
                    //备用名称 直接包含就行
                    break;
                }

                KdyLog.Warn($"影片采集遇到歧义名称，已跳过。第三方名称：{oldKey} 豆瓣名称：{douBanName}");
            }

            if (result.IsSuccess == false)
            {
                return KdyResult.Error<CreateForSubjectIdDto>(KdyResultCode.Error, $"匹配豆瓣信息失败，未找到匹配02。{keyWord}");
            }

            return result;
        }

        /// <summary>
        /// 重试保存图片
        /// </summary>
        /// <remarks>
        /// 任务没保存成功时 手动再重写保存
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> RetrySaveImgAsync(int douBanInfoId)
        {
            var dbDouBanInfo = await _douBanInfoRepository.FirstOrDefaultAsync(a => a.Id == douBanInfoId);
            if (dbDouBanInfo == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "Id错误");
            }

            if (string.IsNullOrEmpty(dbDouBanInfo.VideoImg) ||
                dbDouBanInfo.VideoImg.Contains("doubanio") == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "图片为空或非豆瓣图片 重传失败");
            }

            //上传图片
            var imgResult = await _kdyImgSaveService.PostFileByUrl(dbDouBanInfo.VideoImg);
            if (imgResult.IsSuccess == false)
            {
                return KdyResult.Error(imgResult.Code, imgResult.Msg);
            }

            dbDouBanInfo.VideoImg = imgResult.Data;
            _douBanInfoRepository.Update(dbDouBanInfo);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
    }
}
