using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 图床关联
    /// </summary>
    public class KdyImgSave : BaseEntity<long>
    {
        /// <summary>
        /// 文件Md5长度
        /// </summary>
        public const int FileMd5Length = 32;

        /// <summary>
        /// 图片Url长度
        /// </summary>
        public const int UrlLength = 200;

        /// <summary>  
        /// 文件Md5  
        /// </summary>
        [StringLength(FileMd5Length)]
        public string FileMd5 { get; set; }

        /// <summary>  
        /// Url1  
        /// </summary>  
        [StringLength(UrlLength)]
        public string MainUrl { get; set; }

        /// <summary>  
        /// 地址1  
        /// </summary>  
        [StringLength(UrlLength)]
        public string OneUrl { get; set; }

        /// <summary>  
        /// 地址2  
        /// </summary>  
        [StringLength(UrlLength)]
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
        /// 备用 
        /// </summary>
        [StringLength(UrlLength)]
        public string UrlBack { get; set; }

        /// <summary>
        ///  后期可扩展多个Url 
        /// </summary>
        public string[] Urls { get; set; }
    }
}
