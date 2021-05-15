using AutoMapper;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity;
namespace KdyWeb.Dto.KdyImg
{
    /// <summary>
    /// 分页查询图床Dto
    /// </summary>
    [AutoMap(typeof(KdyImgSave))]
    public class QueryKdyImgDto : CreatedUserDto<long>
    {
        /// <summary>  
        /// 文件Md5  
        /// </summary>
        public string FileMd5 { get; set; }

        /// <summary>  
        /// Url1  
        /// </summary>  
        public string MainUrl { get; set; }

        /// <summary>  
        /// 地址1  
        /// </summary>  
        public string OneUrl { get; set; }

        /// <summary>  
        /// 地址2  
        /// </summary>  
        public string TwoUrl { get; set; }

        /// <summary>  
        /// 用户Id  
        /// </summary>  
        public int? UserId { get; set; }

        /// <summary>  
        /// 用户昵称  
        /// </summary>  
        public string UserNick { get; set; }

        /// <summary>
        ///  后期可扩展多个Url 
        /// </summary>
        public string[] Urls { get; set; }

        /// <summary>
        /// 完整图床路径
        /// </summary>
        public string FullImgUrl { get; set; }
    }
}
