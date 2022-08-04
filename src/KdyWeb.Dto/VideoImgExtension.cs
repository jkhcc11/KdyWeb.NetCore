using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.KdyOptions;
using KdyWeb.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KdyWeb.Dto
{
    /// <summary>
    /// 图片扩展
    /// </summary>
    public static class VideoImgExtension
    {
        /// <summary>
        /// 图片处理
        /// </summary>
        public static void ImgHandler<TDto>(this TDto dto)
        where TDto : class, IBaseImgUrl
        {
            var option = KdyBaseServiceProvider.ServiceProvide.GetService<IOptions<KdySelfHostOption>>();
            dto.VideoImg = dto.VideoImg.GetDouImgName(option?.Value.ProxyHost);
        }
    }
}
