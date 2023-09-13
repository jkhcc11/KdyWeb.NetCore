using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.GameDown;
using KdyWeb.ICommonService.KdyHttp;
using KdyWeb.IService.GameDown;
using KdyWeb.Service.Job;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.GameDown
{
    /// <summary>
    /// Byrut下载相关
    /// </summary>
    /// <remarks>
    /// https://byrut.org/
    /// </remarks>
    public class GameDownWithByrutService : BaseKdyService, IGameDownWithByrutService
    {
        private readonly IKdyRequestClientCommon _kdyRequestClientCommon;
        private readonly IKdyRepository<GameInfoMain, long> _gameInfoMainRepository;

        public GameDownWithByrutService(IUnitOfWork unitOfWork, IKdyRequestClientCommon kdyRequestClientCommon,
            IKdyRepository<GameInfoMain, long> gameInfoMainRepository)
            : base(unitOfWork)
        {
            _kdyRequestClientCommon = kdyRequestClientCommon;
            _gameInfoMainRepository = gameInfoMainRepository;
        }

        /// <summary>
        /// 根据详情创建下载信息
        /// </summary>
        /// <returns></returns>
        public async Task CreateDownInfoByDetailUrlAsync(string detailUrl, string userAgent, string cookie)
        {
            var request = new KdyRequestCommonInput(detailUrl, HttpMethod.Get)
            {
                UserAgent = userAgent,
                TimeOut = 8,
                ExtData = new KdyRequestCommonExtInput(),
                Cookie = cookie
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                if (response.HttpCode == HttpStatusCode.Forbidden)
                {
                    //请求被拦截
                    KdyLog.LogError($"分页Url:{detailUrl}.请求被拦截");

                    //1分钟后重试
                    CreateTaskQueue(false, 1 * 60, detailUrl: detailUrl);
                    return;
                }

                KdyLog.LogWarning($"详情Url:{detailUrl}异常.{response.ToJsonStr()}");
                return;
            }

            #region 解析详情页
            var html = response.Data;
            var youtube = html.GetValueByXpath("//lite-youtube", "videoid");
            string version = html.GetValueByXpath("//div[@class='subhname red']", "text")
                    .Split('[', '&').First(),
                name = html.GetValueByXpath("//a[@class='itemdown_nottorent ubar']", "data-name"),
                size = html.GetValueByXpath("//a[@class='itemdown_nottorent ubar']", "data-size")
                    .Replace("ГБ", "GB").Replace("МБ", "MB"),
                covert = html.GetValueByXpath("//a[@class='itemdown_nottorent ubar']", "data-poster"),
                torrentUrl = html.GetValueByXpath("//div[@class='block_down']//a[@class='itemdown_games']", "href"),

                videoUrl = html.GetValueByXpath("//video/source[@type='video/mp4']", "src");
            if (youtube.IsEmptyExt() == false)
            {
                videoUrl = $"https://www.youtube-nocookie.com/embed/{youtube}?autoplay=1";
            }

            var md5 = (detailUrl + version + videoUrl).Md5Ext(solt: "");
            if (name.IsEmptyExt())
            {
                name = html.GetValueByXpath("//a[@class='itemdown_nottorent ubar']", "data-title");
            }

            var screenNode = html.GetNodeCollection("//div[@class='scrblock']//img");
            var screenList = new List<string>();
            if (screenNode != null)
            {
                foreach (var nodeItem in screenNode)
                {
                    var imgUrl = nodeItem.GetAttributeValue("src", "");
                    if (imgUrl.IsEmptyExt())
                    {
                        continue;
                    }

                    screenList.Add(imgUrl);
                }
            }
            #endregion

            #region 入库
            //db是否存在
            var dbInfo = await _gameInfoMainRepository.GetWriteQuery()
                .FirstOrDefaultAsync(a => a.SourceUrl == detailUrl);
            if (dbInfo == null)
            {
                //新增
                var dbGameInfo = new GameInfoMain(detailUrl, md5, name,
                    size, version
                    , covert, torrentUrl)
                {
                    ScreenCapture = screenList,
                    DetailId = html.GetValueByXpath("//input[@name='post_id']", "value"),
                    UserHash = html.GetValueByXpath("//input[@name='user_hash']", "value"),
                    VideoUrl = videoUrl.Replace("microtrailer", "movie_max"),
                };
                await _gameInfoMainRepository.CreateAsync(dbGameInfo);
                KdyLog.LogDebug($"详情页：{detailUrl},入库成功");
            }
            else
            {
                if (dbInfo.SourceMd5 == md5)
                {
                    KdyLog.LogWarning($"详情Url:{detailUrl} 暂无更新");
                    return;
                }

                dbInfo.SourceMd5 = md5;
                dbInfo.TorrentUrl = torrentUrl;
                dbInfo.GameVersion = version;
                dbInfo.GameSize = size;
                _gameInfoMainRepository.Update(dbInfo);
                KdyLog.LogInformation($"详情页：{detailUrl},更新成功");
            }

            await UnitOfWork.SaveChangesAsync();
            #endregion
        }

        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <remarks>
        /// 根据分页获取详情Url
        /// </remarks>
        /// <returns></returns>
        public async Task QueryPageInfoAsync(int page, string userAgent, string cookie)
        {
            var pageUrl = $"https://byrut.org/page/{page}/";
            if (page <= 1)
            {
                //第一页就是首页
                pageUrl = "https://byrut.org";
            }

            var request = new KdyRequestCommonInput(pageUrl, HttpMethod.Get)
            {
                UserAgent = userAgent,
                TimeOut = 8,
                ExtData = new KdyRequestCommonExtInput(),
                Cookie = cookie,
                IsAutoRedirect = true
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                if (response.HttpCode == HttpStatusCode.Forbidden)
                {
                    //请求被拦截
                    KdyLog.LogError($"分页Url:{pageUrl}.请求被拦截");

                    //被Ban延迟1分钟
                    CreateTaskQueue(true, 1 * 60, page: page);
                    return;
                }

                KdyLog.LogWarning($"分页Url:{pageUrl}异常.{response.ToJsonStr()}");
                return;
            }

            var detailNode = response.Data.GetNodeCollection("//*[@id='dle-content']//div[@class='short_title']/a");
            //每个任务延迟30秒
            var startDelaySecond = 0;
            foreach (var detailNodeItem in detailNode)
            {
                var detailUrl = detailNodeItem.GetAttributeValue("href", "");
                if (detailUrl.IsEmptyExt())
                {
                    continue;
                }

                startDelaySecond += 30;
                CreateTaskQueue(false, startDelaySecond, detailUrl: detailUrl);
            }
        }

        /// <summary>
        /// 根据最大分页查询所有
        /// </summary>
        /// <remarks>
        /// 初始化所有分页从1开始，到达最大页数每个创建一个任务
        /// </remarks>
        /// <returns></returns>
        public async Task QueryAllInfoAsync(int maxPage, string userAgent, string cookie)
        {
            //每次延迟10秒
            var startDelay = 0;
            for (var i = 1; i <= maxPage; i++)
            {
                startDelay += 10;
                CreateTaskQueue(true, startDelay, i);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 根据Id和UseHash获取Stream商店Url
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetSteamStoreUrlByIdAndUserHashAsync(string userAgent, string cookie,
            string customId, string userHash)
        {
            var postData = $"castom=custom+id%3D'{customId}'+template%3D'modulesfull%2Fsteam_updinfo'+cache%3D'no'&user_hash={userHash}";
            var request = new KdyRequestCommonInput("https://byrut.org/engine/ajax/controller.php?mod=ajaxsp", HttpMethod.Post)
            {
                UserAgent = userAgent,
                TimeOut = 8,
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = postData,
                    IsAjax = true
                },
                Cookie = cookie,
            };
            var response = await _kdyRequestClientCommon.SendAsync(request);
            if (response.IsSuccess == false)
            {
                if (response.HttpCode == HttpStatusCode.Forbidden)
                {
                    //请求被拦截
                    throw new KdyCustomException($"获取Steam失败:{customId}|{userHash}.请求被拦截");
                }

                KdyLog.LogWarning($"获取Steam失败:{customId}|{userHash}.异常{response.ToJsonStr()}");
                return default;
            }

            var steamNewsUrl = response.Data.GetValueByXpath("//a", "href");
            //https://store.steampowered.com/news/app/1817070?updates=true
            //转换 https://store.steampowered.com/app/1817070
            if (steamNewsUrl.GetNumber().IsEmptyExt())
            {
                KdyLog.LogWarning($"获取Steam失败:{customId}|{userHash}.无Steam信息");
                return default;
            }

            return steamNewsUrl
                .Replace("?updates=true", "")
                .Replace("/news", "");
        }

        /// <summary>
        /// 根据种子文件转换磁力
        /// </summary>
        /// <returns></returns>
        public Task<ConvertMagnetByByTorrentUrlDto> ConvertMagnetByByTorrentUrlAsync(ConvertMagnetByByTorrentInput input)
        {
            throw new NotImplementedException("文件转磁力为实现");
            //var restClient = new RestClient
            //{
            //    UserAgent = input.UserAgent,
            //};
            //restClient.UseNewtonsoftJson();

            //var request = new RestRequest(input.TorrentUrl, Method.GET);
            //request.AddHeader("Cookie", input.Cookie);
            //request.AddHeader("Referer", input.Referer);
            //var response = await restClient.ExecuteAsync(request);
            //if (response.IsSuccessful == false)
            //{
            //    if (response.StatusCode == HttpStatusCode.Forbidden)
            //    {
            //        if (response.Content == "Access denied")
            //        {
            //            //已更新 重新获取详情
            //            return default;
            //        }

            //        //请求被拦截 重试
            //        throw new KdyCustomException($"下载Url:{input.TorrentUrl}.请求被拦截");
            //    }

            //    KdyLog.LogWarning($"分页Url:{input.TorrentUrl}异常.{response.ToJsonStr()}");
            //    return default;
            //}

            ////解析种子
            //var parser = new BencodeParser();
            //var fileStream = new MemoryStream(response.RawBytes);
            //var torrent = parser.Parse<Torrent>(fileStream);
            //return new ConvertMagnetByByTorrentUrlDto()
            //{
            //    FileName = torrent.DisplayName,
            //    InfoHash = torrent.OriginalInfoHash,
            //    MagnetLink = torrent.GetMagnetLink(MagnetLinkOptions.None)
            //};
        }

        /// <summary>
        /// 创建任务队列
        /// </summary>
        /// <param name="isPageUrl">是否分页</param>
        /// <param name="delaySecond">延迟秒</param>
        /// <param name="page">页数</param>
        /// <param name="detailUrl">详情Url</param>
        private void CreateTaskQueue(bool isPageUrl, int delaySecond, int page = 0, string detailUrl = "")
        {
            var input = new GameDownCaptureJobInput()
            {
                DetailUrl = detailUrl,
                IsPageUrl = isPageUrl,
                Page = page
            };
            BackgroundJob.Schedule<GameDownCaptureJobService>(a => a.ExecuteAsync(input), TimeSpan.FromSeconds(delaySecond));

        }
    }
}
