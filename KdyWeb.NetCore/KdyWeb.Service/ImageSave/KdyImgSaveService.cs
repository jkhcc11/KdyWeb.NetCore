using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Entity;
using KdyWeb.IRepository.ImageSave;
using KdyWeb.IService.ImageSave;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Memory;

namespace KdyWeb.Service.ImageSave
{

    /// <summary>
    /// 图床关联 服务实现
    /// </summary>
    public class KdyImgSaveService : BaseKdyService, IKdyImgSaveService
    {
        private readonly IKdyImgSaveRepository _kdyImgSaveRepository;
        private readonly IMemoryCache _memoryCache;

        public KdyImgSaveService(IKdyImgSaveRepository kdyImgSaveRepository, IMemoryCache memoryCache)
        {
            _kdyImgSaveRepository = kdyImgSaveRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 根据ImgId获取可用图片Url
        /// </summary>
        /// <remarks>
        ///  直接获取返回 不要校验
        /// </remarks>
        /// <param name="imgId">图片Id</param>
        /// <returns></returns>
        public async Task<string> GetImageByImgId(long imgId)
        {
            string cacheKey = $"ImgId_{imgId}",
                value = _memoryCache.Get<string>(cacheKey);
            if (value.IsEmptyExt() == false)
            {
                return value;
            }

            //todo:后期定时校验
            var dbImg = await _kdyImgSaveRepository.FirstOrDefaultAsync(a => a.Id == imgId);
            var url = ImgHandler(dbImg);
            _memoryCache.Set(cacheKey, url, TimeSpan.FromHours(1));

            return url;
        }

        /// <summary>
        /// 图片处理
        /// </summary>
        /// <returns></returns>
        private string ImgHandler(KdyImgSave dbImg)
        {
            var host = GetConfig<string>(KdyWebServiceConst.ImgHostKey);
            var defaultUrl = $"{host}{KdyWebServiceConst.DefaultImgUrl}";

            if (dbImg == null)
            {
                return defaultUrl;
            }

            //没有值
            if (dbImg.MainUrl.IsEmptyExt() &&
                dbImg.TwoUrl.IsEmptyExt() &&
                dbImg.OneUrl.IsEmptyExt() &&
                dbImg.Urls.Any() == false)
            {
                return defaultUrl;
            }

            if (dbImg.MainUrl.IsEmptyExt() == false)
            {
                return dbImg.MainUrl;
            }

            if (dbImg.OneUrl.IsEmptyExt() == false)
            {
                return dbImg.OneUrl;
            }

            if (dbImg.TwoUrl.IsEmptyExt() == false)
            {
                return dbImg.TwoUrl;
            }

            if (dbImg.Urls.Any())
            {
                return $"{host}/{dbImg.Urls.First().TrimStart('/')}";
            }

            return defaultUrl;
        }
    }
}
