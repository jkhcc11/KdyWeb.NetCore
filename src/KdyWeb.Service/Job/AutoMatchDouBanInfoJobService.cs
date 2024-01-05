using System.Threading.Tasks;
using Hangfire;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.HangFire;
using KdyWeb.Dto.Job;
using KdyWeb.IService.HttpApi;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.Job
{
    /// <summary>
    /// 自动匹配豆瓣信息
    /// </summary>
    /// <remarks>
    ///  仅根据关键字匹配豆瓣信息，如果匹配成功(关键字和年份一致)，则保存豆瓣信息
    /// </remarks>
    [Queue(HangFireQueue.DouBan)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 20, 30, 40, 50 })]
    public class AutoMatchDouBanInfoJobService : BaseKdyJob<AutoMatchDouBanInfoJobInput>
    {
        private readonly IDouBanHttpApi _douBanHttpApi;
        private readonly IDouBanInfoService _douBanInfoService;

        public AutoMatchDouBanInfoJobService(IDouBanHttpApi douBanHttpApi, IDouBanInfoService douBanInfoService)
        {
            _douBanHttpApi = douBanHttpApi;
            _douBanInfoService = douBanInfoService;
        }

        /// <summary>
        /// 具体执行 异步
        /// </summary>
        public override async Task ExecuteAsync(AutoMatchDouBanInfoJobInput input)
        {
            var searchResult = await _douBanHttpApi.SearchSuggestAsync(input.VodTitle);
            if (searchResult.IsSuccess == false)
            {
                searchResult = await _douBanHttpApi.KeyWordSearchAsync(input.VodTitle, 1);
            }

            if (searchResult.IsSuccess == false)
            {
                KdyLog.LogWarning("影片关键字：{0}，匹配豆瓣失败，无法检索到豆瓣信息。", input.VodTitle);
                return;
            }

            //开始匹配
            foreach (var searchItem in searchResult.Data)
            {
                var douBanTitle = searchItem.Title;
                var vodTitle = input.VodTitle;
                if (douBanTitle.RemoveSpecialCharacters() != vodTitle.RemoveSpecialCharacters() &&
                    searchItem.Year != input.VodYear)
                {
                    continue;
                }

                //存豆瓣信息
                var douBanInfo = await _douBanInfoService.CreateForSubjectIdAsync(searchItem.SubjectId);
                if (douBanInfo.IsSuccess == false)
                {
                    KdyLog.LogWarning("豆瓣主题Id:{subject}，获取豆瓣信息异常，异常信息：{errorMsg}"
                        , searchItem.SubjectId
                        , douBanInfo.Msg);

                    if (douBanInfo.Code == KdyResultCode.HttpError)
                    {
                        throw new KdyCustomException(douBanInfo.Msg);
                    }
                }
                else
                {
                    //豆瓣信息保存成功，开始绑定影片
                    var bindInput = new BindVodDouBanInfoJobInput()
                    {
                        DouBanId = douBanInfo.Data.Id,
                        MainId = input.MainId
                    };

                    BackgroundJob.Enqueue<BindVodDouBanInfoJobService>(a => a.ExecuteAsync(bindInput));
                }

                return;
            }
        }
    }
}
