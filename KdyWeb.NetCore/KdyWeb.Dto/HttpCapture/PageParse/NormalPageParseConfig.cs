using System.Net.Http;
using KdyWeb.Entity.HttpCapture;
using KdyWeb.PageParse;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 通用站点页面解析配置
    /// </summary>
    public class NormalPageParseConfig : IKdyWebPageParseConfig<BaseSearchConfig, BasePageConfig>
    {
        public string BaseHost { get; set; }

        public string UserAgent { get; set; }

        public BaseSearchConfig SearchConfig { get; set; }

        public BasePageConfig PageConfig { get; set; }
    }

    public static class NormalPageParseConfigExtension
    {
        /// <summary>
        /// 数据库配置=>通用配置
        /// </summary>
        /// <returns></returns>
        public static NormalPageParseConfig ToNormalPageParseConfig(this PageSearchConfig dbConfig)
        {
            var config = new NormalPageParseConfig
            {
                BaseHost = dbConfig.BaseHost,
                UserAgent = dbConfig.UserAgent,
                SearchConfig = new BaseSearchConfig()
                {
                    Method = dbConfig.ConfigHttpMethod == ConfigHttpMethod.Post ? HttpMethod.Post : HttpMethod.Get,
                    SearchPath = dbConfig.SearchPath,
                    SearchData = dbConfig.SearchData,
                    SearchXpath = dbConfig.SearchXpath,
                    EndXpath = dbConfig.EndXpath,
                    NotEndKey = dbConfig.NotEndKey,
                    NameAttr = dbConfig.NameAttr,
                    ImgAttr = dbConfig.ImgAttr
                },
                PageConfig = new BasePageConfig()
                {
                    DetailXpath = dbConfig.DetailXpath,
                    ImgXpath = dbConfig.DetailImgXpath,
                    NameXpath = dbConfig.DetailNameXpath,
                    EndXpath = dbConfig.DetailEndXpath,
                }
            };

            return config;
        }
    }
}
