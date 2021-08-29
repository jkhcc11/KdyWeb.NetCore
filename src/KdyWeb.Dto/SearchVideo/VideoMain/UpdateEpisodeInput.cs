using System.ComponentModel.DataAnnotations;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 更新剧集Input
    /// </summary>
    public class UpdateEpisodeInput : EditEpisodeItem
    {
        /// <summary>
        /// 剧集Id
        /// </summary>
        [Required(ErrorMessage = "剧集Id必填")]
        public long Id { get; set; }
    }

    public static class UpdateEpisodeInputException
    {
        /// <summary>
        /// Input 转 Db
        /// </summary>
        /// <param name="input">更新剧集Input</param>
        /// <param name="dbEpisode">剧集</param>
        public static void ToDbEpisode(this UpdateEpisodeInput input, VideoEpisode dbEpisode)
        {
            if (input == null)
            {
                return;
            }

            dbEpisode.EpisodeUrl = input.EpisodeUrl;
            dbEpisode.EpisodeName = input.EpisodeName;
            if (input.OrderBy != null)
            {
                dbEpisode.OrderBy = input.OrderBy.Value;
            }
        }
    }
}
