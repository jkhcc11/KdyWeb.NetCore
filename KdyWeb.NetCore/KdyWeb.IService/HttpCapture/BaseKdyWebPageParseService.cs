using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.HttpCapture;
using KdyWeb.Dto.KdyHttp;
using KdyWeb.IService.KdyHttp;
using KdyWeb.PageParse;
using KdyWeb.Utility;

namespace KdyWeb.IService.HttpCapture
{
    /// <summary>
    /// 通用站点页面解析 抽象基类
    /// </summary>
    public abstract class BaseKdyWebPageParseService : BaseKdyWebPageParseService<NormalPageParseConfig, KdyWebPageSearchInput, 
        KdyWebPageSearchOut, KdyWebPageSearchOutItem, 
        KdyWebPagePageInput, KdyWebPagePageOut, 
        NormalPageParseOut, NormalPageParseInput, BaseSearchConfig, BasePageConfig>
    {
        protected BaseKdyWebPageParseService(IKdyRequestClientCommon kdyRequestClientCommon) :
            base(kdyRequestClientCommon)
        {

        }
    }

    /// <summary>
    /// 站点页面解析 抽象基类
    /// </summary>
    /// <typeparam name="TSearchInput">搜索入参泛型</typeparam>
    /// <typeparam name="TSearchOut">搜索出参泛型</typeparam>
    /// <typeparam name="TSearchItem">搜索出参Item泛型</typeparam>
    /// <typeparam name="TPageInput">页面解析入参</typeparam>
    /// <typeparam name="TPageOut">页面解析出参</typeparam>
    /// <typeparam name="TConfig">站点页面解析配置 </typeparam>
    /// <typeparam name="TPageParseOut">页面解析出参  <see cref="IPageParseOut"/></typeparam>
    /// <typeparam name="TPageParseInput">页面解析入参 <see cref="IPageParseInput"/> </typeparam>
    /// <typeparam name="TSearchConfig">搜索配置</typeparam>
    /// <typeparam name="TPageConfig">详情配置</typeparam>
    public abstract class BaseKdyWebPageParseService<TConfig, TSearchInput, TSearchOut, TSearchItem, TPageInput, TPageOut, TPageParseOut, TPageParseInput, TSearchConfig, TPageConfig> :
        IPageParseService<TPageParseOut, TPageParseInput>,
        IKdyWebPageParseService<TSearchInput, TSearchOut, TSearchItem, TPageInput, TPageOut>
        where TSearchInput : IKdyWebPageSearchInput, new()
        where TSearchOut : IKdyWebPageSearchOut<TSearchItem>
        where TPageInput : IKdyWebPagePageInput, new()
        where TPageOut : IKdyWebPagePageOut
        where TSearchItem : IKdyWebPageSearchOutItem, new()
        where TConfig : class, IKdyWebPageParseConfig<TSearchConfig, TPageConfig>, new()
        where TPageParseInput : IPageParseInput, new()
        where TPageParseOut : IPageParseOut, new()
        where TSearchConfig : ISearchConfig, new()
        where TPageConfig : IPageConfig, new()
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        protected TConfig BaseConfig;
        protected readonly IKdyRequestClientCommon KdyRequestClientCommon;

        protected BaseKdyWebPageParseService(IKdyRequestClientCommon kdyRequestClientCommon)
        {
            KdyRequestClientCommon = kdyRequestClientCommon;
        }

        #region 搜索处理

        /// <summary>
        /// 搜索获取源Html
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<KdyRequestCommonResult>> SendSearchAsync(TSearchInput input)
        {
            var postData = string.Empty;
            var searchUrl = $"{BaseConfig.BaseHost}{BaseConfig.SearchConfig.SearchPath}";
            if (BaseConfig.SearchConfig.Method == HttpMethod.Get)
            {
                //Get直接格式化
                searchUrl = string.Format(searchUrl, input.KeyWord);
            }
            else
            {
                postData = string.Format(BaseConfig.SearchConfig.SearchData, input.KeyWord);
            }

            var reqInput = new KdyRequestCommonInput(searchUrl, BaseConfig.SearchConfig.Method)
            {
                UserAgent = BaseConfig.UserAgent,
                TimeOut = 3000,
                ExtData = new KdyRequestCommonExtInput()
                {
                    PostData = postData
                }
            };

            //请求
            var reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyRequestCommonResult>(KdyResultCode.Error, $"解析失败，搜索失败.{reqResult.ErrMsg}");
            }

            return KdyResult.Success(reqResult);
        }

        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<TSearchOut>> GetSearchResultAsync(TSearchInput input)
        {
            var sendSearchResult = await SendSearchAsync(input);
            if (sendSearchResult.IsSuccess == false)
            {
                return KdyResult.Error<TSearchOut>(sendSearchResult.Code, sendSearchResult.Msg);
            }

            return SearchResultHandler(sendSearchResult.Data);
        }

        /// <summary>
        /// 搜索结果处理
        /// </summary>
        /// <returns></returns>
        public abstract KdyResult<TSearchOut> SearchResultHandler(KdyRequestCommonResult searchResult);

        #endregion

        #region 详情页处理
        /// <summary>
        /// 详情页获取源Html
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<KdyRequestCommonResult>> SendDetailAsync(TSearchItem searchOut, TPageInput input)
        {
            var reqInput = new KdyRequestCommonInput(input.DetailUrl, HttpMethod.Get)
            {
                UserAgent = BaseConfig.UserAgent,
                TimeOut = 3000
            };

            //请求
            var reqResult = await KdyRequestClientCommon.SendAsync(reqInput);
            if (reqResult.IsSuccess == false)
            {
                return KdyResult.Error<KdyRequestCommonResult>(KdyResultCode.Error, $"解析失败，获取详情失败.{reqResult.ErrMsg}");
            }

            return KdyResult.Success(reqResult);
        }

        /// <summary>
        /// 获取页面解析结果
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<List<TPageOut>>> GetPageResultAsync(TSearchItem searchOut, TPageInput input)
        {
            var sendSearchResult = await SendDetailAsync(searchOut, input);
            if (sendSearchResult.IsSuccess == false)
            {
                return KdyResult.Error<List<TPageOut>>(sendSearchResult.Code, sendSearchResult.Msg);
            }

            return DetailResultHandler(searchOut, sendSearchResult.Data);
        }

        /// <summary>
        /// 详情页处理
        /// </summary>
        /// <returns></returns>
        public abstract KdyResult<List<TPageOut>> DetailResultHandler(TSearchItem searchOut, KdyRequestCommonResult searchResult);

        #endregion

        #region 结果处理
        public async Task<KdyResult<TPageParseOut>> GetResultAsync(TPageParseInput input)
        {
            BaseConfig = await GetConfigAsync(input.ConfigId);
            if (string.IsNullOrEmpty(input.KeyWord) &&
                string.IsNullOrEmpty(input.Detail))
            {
                return KdyResult.Error<TPageParseOut>(KdyResultCode.ParError, "关键字和详情页不能同时为空");
            }

            if (string.IsNullOrEmpty(input.Detail) == false)
            {
                #region 详情处理
                var firstData = new TSearchItem()
                {
                    DetailUrl = input.Detail
                };

                //获取详情页结果
                var second = await GetPageResultAsync(firstData, new TPageInput()
                {
                    DetailUrl = firstData.DetailUrl
                });
                if (second.IsSuccess == false)
                {
                    return KdyResult.Error<TPageParseOut>(second.Code, second.Msg);
                }

                return DetailHandler(firstData, second.Data);
                #endregion
            }

            #region 关键字处理
            //搜索结果
            var first = await GetSearchResultAsync(new TSearchInput()
            {
                KeyWord = input.KeyWord
            });
            if (first.IsSuccess == false)
            {
                return KdyResult.Error<TPageParseOut>(first.Code, first.Msg);
            }

            return await KeyWordHandler(first.Data.Items);
            #endregion
        }

        /// <summary>
        /// 关键字结果处理
        /// </summary>
        /// <param name="searchItems">搜索结果列表</param>
        /// <returns></returns>
        public abstract Task<KdyResult<TPageParseOut>> KeyWordHandler(IList<TSearchItem> searchItems);

        /// <summary>
        /// 详情结果处理
        /// </summary>
        /// <param name="searchItem">详情信息</param>
        /// <param name="detailResult">详情结果列表</param>
        /// <returns></returns>
        public abstract KdyResult<TPageParseOut> DetailHandler(TSearchItem searchItem, List<TPageOut> detailResult);

        #endregion

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        protected abstract Task<TConfig> GetConfigAsync(long configId);

        /// <summary>
        /// 图片处理
        /// </summary>
        /// <returns></returns>
        protected string ImgHandler(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl))
            {
                return string.Empty;
            }

            if (imgUrl.StartsWith("http"))
            {
                return imgUrl;
            }

            var temp = imgUrl.GetStrMathExt("http://", ".jpg");
            if (string.IsNullOrEmpty(temp))
            {
                temp = imgUrl.GetStrMathExt("https://", ".jpg");
            }

            if (string.IsNullOrEmpty(temp) == false)
            {
                return $"http://{temp}.jpg";
            }

            return imgUrl.GetStrMathExt("\\(", "\\)");
        }

        /// <summary>
        /// 名称处理
        /// </summary>
        /// <returns></returns>
        protected string NameHandler(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            if (name.EndsWith("完结"))
            {
                return name.RemoveStrExt("完结");
            }

            if (name.Contains("更新至"))
            {
                var index = name.IndexOf("更新至", StringComparison.Ordinal);
                return name.Substring(0, index);
            }

            if (name.Contains("连载至"))
            {
                var index = name.IndexOf("连载至", StringComparison.Ordinal);
                return name.Substring(0, index);
            }

            return name;
        }

    }
}
