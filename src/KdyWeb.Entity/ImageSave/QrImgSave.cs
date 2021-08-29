using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity
{
    /// <summary>
    /// 二维码多图
    /// </summary>
    public class QrImgSave : BaseEntity<long>
    {
        /// <summary>  
        /// 用户Id  
        /// </summary>  
        public long? UserId { get; set; }

        /// <summary>  
        /// Md5值  
        /// </summary>
        [StringLength(KdyImgSave.FileMd5Length)]
        public string FileMd5 { get; set; }

        /// <summary>  
        /// 图片路径  
        /// </summary>  
        public string[] ImgPath { get; set; }
    }
}
