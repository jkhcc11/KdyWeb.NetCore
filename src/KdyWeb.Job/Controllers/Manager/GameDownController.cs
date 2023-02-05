﻿using System;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto.GameDown;
using KdyWeb.Dto.Job;
using KdyWeb.Entity.GameDown;
using KdyWeb.EntityFramework.Migrations;
using KdyWeb.IService.GameDown;
using KdyWeb.Service.Job;
using KdyWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Job.Controllers.Manager
{
    /// <summary>
    /// 游戏下载地址
    /// </summary>
    public class GameDownController : BaseManagerController
    {
        private readonly IKdyRepository<GameInfoMain, long> _gameInfoRepository;
        private readonly IGameDownService _gameDownService;

        public GameDownController(IGameDownService gameDownService,
            IKdyRepository<GameInfoMain, long> gameInfoRepository)
        {
            _gameDownService = gameDownService;
            _gameInfoRepository = gameInfoRepository;
        }

        /// <summary>
        /// 查询下载地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(KdyResult<PageList<QueryGameDownListWithAdminDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> QueryGameDownListWithAdminAsync([FromQuery] QueryGameDownListWithAdminInput input)
        {
            var result = await _gameDownService.QueryGameDownListWithAdminAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-down-url/{downId}")]
        [ProducesResponseType(typeof(KdyResult<PageList<GetDownMagnetByDownIdDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDownMagnetByDownIdAsync(long downId)
        {
            var result = await _gameDownService.GetDownMagnetByDownIdAsync(downId);
            return Ok(result);
        }

        /// <summary>
        /// 转换steam信息(test)
        /// </summary>
        /// <returns></returns>
        [HttpGet("covert-steam")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CovertSteam()
        {
            var result = await _gameInfoRepository.GetAsNoTracking()
                .ToListAsync();
            var startDelay = 0;
            foreach (var item in result)
            {
                startDelay += 30;

                if (item.Magnet.IsEmptyExt())
                {
                    //转换磁力
                    var convertInput = new TorrentCovertMagnetJobInput()
                    {
                        DownId = item.Id,
                        Referer = item.SourceUrl,
                        TorrentUrl = item.TorrentUrl
                    };
                    BackgroundJob.Schedule<TorrentCovertMagnetJobService>(a => a.ExecuteAsync(convertInput), TimeSpan.FromSeconds(startDelay));

                }

                startDelay += 5;
                if (item.IsHaveSteamInfo() == false)
                {
                    //steam信息
                    var steamJobInput = new SteamInfoJobInput()
                    {
                        DownId = item.Id,
                        CustomId = item.DetailId,
                        UserHash = item.UserHash
                    };
                    BackgroundJob.Schedule<SteamInfoJobService>(a => a.ExecuteAsync(steamJobInput), TimeSpan.FromSeconds(startDelay));
                }
            }

            return Ok($"时长：{startDelay} 秒");
        }
    }
}