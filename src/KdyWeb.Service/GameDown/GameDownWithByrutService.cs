using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.Job;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.Entity.GameDown;
using KdyWeb.IService.GameDown;
using KdyWeb.IService.KdyHttp;
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
                TimeOut = 5000,
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
            string version = html.GetValueByXpath("//div[@class='subhname red']", "text").Split('[').First(),
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
            foreach (var nodeItem in screenNode)
            {
                var imgUrl = nodeItem.GetAttributeValue("src", "");
                if (imgUrl.IsEmptyExt())
                {
                    continue;
                }

                screenList.Add(imgUrl);
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
                dbInfo.VideoUrl = videoUrl.Replace("microtrailer", "movie_max");
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
                TimeOut = 5000,
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
