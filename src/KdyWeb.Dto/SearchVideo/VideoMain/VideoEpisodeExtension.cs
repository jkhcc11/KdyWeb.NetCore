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

        /// <summary>
        /// 获取剧集编辑信息
        /// </summary>
        /// <remarks>
        /// 录入Id    <br/>
        ///    id > 0 不存在db时，db移除   <br/>
        ///    id 小于 0 不存在db时，db新增  <br/>
        /// 录入Id 存在db时，更新db
        /// </remarks>
        /// <param name="dbEpisode">现有数据库剧集信息</param>
        /// <param name="inputEpInfo">录入剧集信息</param>
        /// <param name="videoEpisodeGroup">剧集组Id</param>
        /// <returns></returns>
        public static void EpisodeUpdate(this List<UpdateEpisodeInput> inputEpInfo,
            ICollection<VideoEpisode> dbEpisode, VideoEpisodeGroup videoEpisodeGroup)
        {
            var dbEpIds = dbEpisode.Select(a => a.Id).ToArray();
            var inputEpIds = inputEpInfo.Where(a => a.Id > 0).Select(a => a.Id).ToArray();
            //移除的
            var removeIds = dbEpIds.Except(inputEpIds);
            foreach (var removeId in removeIds)
            {
                var removeItem = dbEpisode.First(a => a.Id == removeId);
                removeItem.IsDelete = true;
            }

            //新增的
            if (inputEpInfo.Any(a => a.Id <= 0))
            {
                foreach (var addEpisode in inputEpInfo
                             .Where(a => a.Id <= 0)
                             .Select(item => new VideoEpisode(item.EpisodeName, item.EpisodeUrl)
                             {
                                 EpisodeGroupId = videoEpisodeGroup.Id,
                                 OrderBy = item.OrderBy ?? 0
                             }))
                {
                    dbEpisode.Add(addEpisode);
                }
            }

            //更新的
            var updateIds = dbEpIds.Intersect(inputEpIds).ToArray();
            foreach (var dbEpItem in dbEpisode.Where(a => updateIds.Contains(a.Id)))
            {
                var inputEpItem = inputEpInfo.First(a => a.Id == dbEpItem.Id);
                dbEpItem.EpisodeName = inputEpItem.EpisodeName;
                dbEpItem.EpisodeUrl = inputEpItem.EpisodeUrl;
                dbEpItem.OrderBy = inputEpItem.OrderBy ?? 0;
            }
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
