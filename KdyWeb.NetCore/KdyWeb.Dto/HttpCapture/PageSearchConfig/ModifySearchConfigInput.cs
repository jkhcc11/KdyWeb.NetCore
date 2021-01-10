using AutoMapper;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 修改搜索配置 Input
    /// </summary>
    public class ModifySearchConfigInput : BaseSearchConfigInput
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        public long ConfigId { get; set; }

        /// <summary>
        /// 修改->数据库
        /// </summary>
        public static void ToDbConfig(PageSearchConfig dbConfig, BaseSearchConfigInput input)
        {
            dbConfig.ServiceFullName = input.ServiceFullName;
            dbConfig.HostName = input.HostName;
            dbConfig.BaseHost = input.BaseHost;
            dbConfig.OtherHost = input.OtherHost;
            dbConfig.UserAgent = input.UserAgent;
            dbConfig.ConfigHttpMethod = input.ConfigHttpMethod;
            dbConfig.SearchPath = input.SearchPath;
            dbConfig.SearchData = input.SearchData;
            dbConfig.SearchXpath = input.SearchXpath;
            dbConfig.EndXpath = input.EndXpath;
            dbConfig.NotEndKey = input.NotEndKey;
            dbConfig.ImgAttr = input.ImgAttr;
            dbConfig.NameAttr = input.NameAttr;
            dbConfig.DetailXpath = input.DetailXpath;
            dbConfig.DetailImgXpath = input.DetailImgXpath;
            dbConfig.DetailNameXpath = input.DetailNameXpath;
            dbConfig.DetailEndXpath = input.DetailEndXpath;
            dbConfig.HostRemark = input.HostRemark;

            dbConfig.PlayUrlSuffix = input.PlayUrlSuffix;
            dbConfig.CaptureDetailUrl = input.CaptureDetailUrl;
            dbConfig.CaptureDetailXpath = input.CaptureDetailXpath;
            dbConfig.CaptureDetailNameSplit = input.CaptureDetailNameSplit;
        }
    }
}
