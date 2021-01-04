using System;
using System.Collections.Generic;
using System.Linq;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 影片剧集扩展
    /// </summary>
    public static class VideoEpisodeExtension
    {
        /// <summary>
        /// 获取剧集编辑信息
        /// </summary>
        /// <remarks>
        /// 根据影片已有剧集和录入剧集对比 <br/>
        /// 已存在则返回更新列表<br/>
        /// 不存在则返回新增列表<br/>
        /// </remarks>
        /// <param name="dbEpisode">现有数据库剧集信息</param>
        /// <param name="inputEpInfo">录入剧集信息</param>
        /// <returns></returns>
        public static GetEditInfoExtOut GetEditInfoExt(this List<EditEpisodeItem> inputEpInfo, List<VideoEpisode> dbEpisode)
        {
            var result = new GetEditInfoExtOut();

            var groupId = dbEpisode.First().EpisodeGroupId;
            var dbEpNames = dbEpisode.Select(a => a.EpisodeName).ToList();

            //新增的剧集
            var canAddEp = inputEpInfo
                .Where(a => dbEpNames.Contains(a.EpisodeName) == false)
                .ToList();
            result.AddEpInfo = canAddEp.Select(episodeItem => new VideoEpisode(episodeItem.EpisodeName, episodeItem.EpisodeUrl)
            {
                OrderBy = episodeItem.OrderBy ?? 0,
                EpisodeGroupId = groupId
            }).ToList();

            //更新的剧集
            var canEditEp = inputEpInfo
                .Where(a => dbEpNames.Contains(a.EpisodeName))
                .ToList();
            result.UpdateEpInfo = new List<VideoEpisode>();
            foreach (var epItem in canEditEp)
            {
                var dbItem = dbEpisode.FirstOrDefault(a => a.EpisodeName == epItem.EpisodeName);
                if (dbItem == null)
                {
                    continue;
                }

                dbItem.EpisodeUrl = epItem.EpisodeUrl;
                result.UpdateEpInfo.Add(dbItem);
            }

            return result;
        }
    }

    /// <summary>
    /// 获取剧集编辑信息扩展 Out
    /// </summary>
    public class GetEditInfoExtOut
    {
        /// <summary>
        /// 新增列表
        /// </summary>
        public List<VideoEpisode> AddEpInfo { get; set; }

        /// <summary>
        /// 更新列表
        /// </summary>
        public List<VideoEpisode> UpdateEpInfo { get; set; }
    }

}
