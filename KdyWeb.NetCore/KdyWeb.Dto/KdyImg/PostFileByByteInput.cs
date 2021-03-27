using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KdyWeb.Dto.KdyImg
{
    /// <summary>
    /// 通过Byte上传 Input
    /// </summary>
    public class PostFileByByteInput
    {
        /// <summary>
        /// 文件窗体
        /// </summary>
        [Required(ErrorMessage = "请选择上传文件")]
        public IFormFile KdyFile { get; set; }
    }

    /// <summary>
    /// 通过Url上传 Input
    /// </summary>
    public class PostFileByUrlInput
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        [Required(ErrorMessage = "请输入图片地址")]
        public string ImgUrl { get; set; }
    }
}
