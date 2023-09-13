using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.HttpApi.Steam;
using KdyWeb.Dto.Job;
using KdyWeb.Entity.GameDown;
using KdyWeb.IService.GameDown;
using KdyWeb.Service.Job;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.GameDown
{
    /// <summary>
    /// 游戏下载资源 服务
    /// </summary>
    public class GameDownService : BaseKdyService, IGameDownService
    {
        private readonly IKdyRepository<GameInfoMain, long> _gameInfoRepository;
        public GameDownService(IUnitOfWork unitOfWork, IKdyRepository<GameInfoMain, long> gameInfoRepository)
            : base(unitOfWork)
        {
            _gameInfoRepository = gameInfoRepository;
        }

        /// <summary>
        /// 查询游戏下载列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryGameDownListWithAdminDto>>> QueryGameDownListWithAdminAsync(QueryGameDownListWithAdminInput input)
        {
            if (input.OrderBy == null ||
                input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new()
                    {
                        Key = $"{nameof(GameInfoMain.ModifyTime)}",
                        OrderBy = KdyEfOrderBy.Desc
                    },
                    new()
                    {
                        Key = $"{nameof(GameInfoMain.CreatedTime)}",
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var query = _gameInfoRepository.GetAsNoTracking();
            if (input.KeyWord.IsEmptyExt() == false &&
                (input.KeyWord.StartsWith("https://") ||
                input.KeyWord.StartsWith("http://")))
            {
                var steamId = input.KeyWord.GetNumber();
                if (steamId.IsEmptyExt() == false)
                {
                    query = query.Where(a => a.SteamId == steamId);
                }

                input.KeyWord = string.Empty;
            }

            var result = await query.GetDtoPageListAsync<GameInfoMain, QueryGameDownListWithAdminDto>(input);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 根据下载Id获取磁力下载
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <returns></returns>
        public async Task<KdyResult<GetDownMagnetByDownIdDto>> GetDownMagnetByDownIdAsync(long downId)
        {
            var result = new GetDownMagnetByDownIdDto();
            var downInfo = await _gameInfoRepository.FirstOrDefaultAsync(a => a.Id == downId);
            if (downInfo.Magnet.IsEmptyExt() == false)
            {
                result.IsMagnet = true;
                result.DownUrl = downInfo.Magnet;
            }
            else
            {
                #region 转换
                result.DownUrl = downInfo.TorrentUrl;

                //转换磁力
                var convertInput = new TorrentCovertMagnetJobInput()
                {
                    DownId = downId,
                    Referer = downInfo.SourceUrl,
                    TorrentUrl = downInfo.TorrentUrl
                };
                BackgroundJob.Schedule<TorrentCovertMagnetJobService>(a => a.ExecuteAsync(convertInput), TimeSpan.FromSeconds(30));

                #endregion
            }

            #region Steam信息
            if (downInfo.SteamUrl.IsEmptyExt())
            {
                //steam信息
                var steamJobInput = new SteamInfoJobInput()
                {
                    DownId = downId,
                    CustomId = downInfo.DetailId,
                    UserHash = downInfo.UserHash
                };
                BackgroundJob.Schedule<SteamInfoJobService>(a => a.ExecuteAsync(steamJobInput), TimeSpan.FromSeconds(40));
            }
            else if (downInfo.SteamUrl.IsEmptyExt() == false)
            {
                var steamJobInput = new SteamInfoJobInput()
                {
                    DownId = downId,
                    SteamUrl = downInfo.SteamUrl,
                    IsSteamUrl = true
                };
                BackgroundJob.Schedule<SteamInfoJobService>(a => a.ExecuteAsync(steamJobInput), TimeSpan.FromSeconds(40));
            }
            #endregion

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 根据Id和磁力更新下载信息
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <param name="magnetLink">磁力链接</param>
        /// <returns></returns>
        public async Task<KdyResult> SaveMagnetByTorrentUrlAsync(long downId, string magnetLink)
        {
            var downInfo = await _gameInfoRepository.FirstOrDefaultAsync(a => a.Id == downId);
            downInfo.SetMagnet(magnetLink);
            _gameInfoRepository.Update(downInfo);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success($"成功：{downId}");
        }

        /// <summary>
        /// 根据downId更新steam信息
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <param name="steamResponse">steam返回信息</param>
        /// <returns></returns>
        public async Task<KdyResult> SaveSteamInfoByDownIdAsync(long downId, GetGameInfoByStoreUrlResponse steamResponse)
        {
            var downInfo = await _gameInfoRepository.FirstOrDefaultAsync(a => a.Id == downId);
            downInfo.SetSteamUrl(steamResponse.DetailUrl);
            downInfo.ChineseName = steamResponse.GameName;
            downInfo.GameCovert = steamResponse.CovertUrl;
            downInfo.Description = steamResponse.Description;
            if (steamResponse.MovieUrlList.Any())
            {
                //保留源站
                downInfo.MovieList = steamResponse.MovieUrlList;
                downInfo.VideoUrl = steamResponse.MovieUrlList.First();
            }

            if (steamResponse.ScreenshotList.Any())
            {
                downInfo.ScreenCapture = steamResponse.ScreenshotList;
            }

            if (steamResponse.SupperLanguage.Any())
            {
                downInfo.ExtInfo = steamResponse.SupperLanguage
                    .ToDictionary(a => a.Key, a => a.Value.ToString());
            }

            _gameInfoRepository.Update(downInfo);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success($"成功：{downId}");
        }

        /// <summary>
        /// 根据DownId直接更新steamUrl
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <param name="steamUrl">steam地址</param>
        /// <returns></returns>
        public async Task SaveSteamUrlAsync(long downId, string steamUrl)
        {
            var downInfo = await _gameInfoRepository.FirstOrDefaultAsync(a => a.Id == downId);
            downInfo.SetSteamUrl(steamUrl);
            _gameInfoRepository.Update(downInfo);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据Id更新下载信息
        /// </summary>
        /// <param name="downId">下载Id</param>
        /// <returns></returns>
        public async Task FaceUpdateGameDownInfoByDownIdAsync(long downId)
        {
            var downInfo = await _gameInfoRepository.FirstOrDefaultAsync(a => a.Id == downId);
            var input = new GameDownCaptureJobInput()
            {
                DetailUrl = downInfo.SourceUrl
            };
            var jobId = BackgroundJob.Enqueue<GameDownCaptureJobService>(a => a.ExecuteAsync(input));
            KdyLog.LogInformation($"DownId:{downId}，更新任务ID:{jobId}");
        }
    }
}
