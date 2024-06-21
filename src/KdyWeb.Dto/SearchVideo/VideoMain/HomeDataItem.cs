using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;
using System.Collections.Generic;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 首页Item
    /// </summary>
    public class HomeDataItem
    {
        /// <summary>
        /// 类型值
        /// </summary>
        /// <remarks>
        /// 如果IsUrl 为True 则这里是对应的路由页面名称
        /// </remarks>
        public string TypeValue { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 类型对应的数据
        /// </summary>
        public IEnumerable<HomeTypeDataItem> TypeDataItems { get; set; } = new List<HomeTypeDataItem>();

        /// <summary>
        /// 是否Url
        /// </summary>
        public bool IsUrl { get; set; }
    }

    /// <summary>
    /// 数据Item
    /// </summary>
    public class HomeTypeDataItem : CreatedUserDto<long>, IBaseImgUrl
    {
        /// <summary>
        /// 视频名称
        /// </summary>
        public string KeyWord { get; set; }

        public string VideoImg { get; set; }

        /// <summary>
        /// 豆瓣评分
        /// </summary>
        public double VideoDouBan { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int VideoYear { get; set; }

        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailUrl { get; set; }
    }
}
