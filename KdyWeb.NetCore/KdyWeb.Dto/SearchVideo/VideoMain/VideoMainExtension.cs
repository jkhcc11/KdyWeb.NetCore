using KdyWeb.Dto.HttpCapture;
using KdyWeb.Entity.OldVideo;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    public static class VideoMainExtension
    {
        /// <summary>
        /// 豆瓣信息->影片主表
        /// </summary>
        public static void ToVideoMain(this VideoMain videoMain, DouBanInfo douBanInfo)
        {
            videoMain.Aka = douBanInfo.Aka;
            videoMain.VideoDouBan = douBanInfo.VideoRating;
            videoMain.VideoYear = douBanInfo.VideoYear;
            videoMain.VideoInfoUrl = $"//movie.douban.com/subject/{douBanInfo.VideoDetailId}/";
            videoMain.VideoMainInfo = new VideoMainInfo(douBanInfo.VideoGenres, douBanInfo.VideoCasts, douBanInfo.VideoDirectors, douBanInfo.VideoCountries)
            {
                VideoSummary = douBanInfo.VideoSummary
            };
        }

        /// <summary>
        /// 旧影视->影片主表
        /// </summary>
        public static void ToVideoMain(this VideoMain videoMain, OldSearchSysMain oldSearchSysMain)
        {
            //videoMain.Aka = douBanInfo.Aka;
            videoMain.VideoDouBan = oldSearchSysMain.VideoDouBan ?? 0;
            videoMain.VideoYear = oldSearchSysMain.VideoYear ?? 0;
            videoMain.VideoInfoUrl = oldSearchSysMain.VideoDetail;
            videoMain.VideoMainInfo = new VideoMainInfo(oldSearchSysMain.VideoType, oldSearchSysMain.VideoCasts, oldSearchSysMain.VideoDirectors, oldSearchSysMain.VideoCountries)
            {
                VideoSummary = oldSearchSysMain.VideoDescribe
            };
            videoMain.OldKeyId = oldSearchSysMain.Id;
        }

        /// <summary>
        /// 豆瓣详情信息->影片主表
        /// </summary>
        public static void ToVideoMain(this VideoMain videoMain, CreateForSubjectIdDto douBanInfo)
        {
            videoMain.Aka = douBanInfo.Aka;
            videoMain.VideoDouBan = douBanInfo.VideoRating;
            videoMain.VideoYear = douBanInfo.VideoYear;
            videoMain.VideoInfoUrl = $"//movie.douban.com/subject/{douBanInfo.VideoDetailId}/";
            videoMain.VideoMainInfo = new VideoMainInfo(douBanInfo.VideoGenres, douBanInfo.VideoCasts, douBanInfo.VideoDirectors, douBanInfo.VideoCountries)
            {
                VideoSummary = douBanInfo.VideoSummary
            };
        }
    }
}
