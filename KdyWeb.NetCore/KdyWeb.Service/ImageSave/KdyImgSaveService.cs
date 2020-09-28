using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Entity;
using KdyWeb.IRepository.ImageSave;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.KdyFile;
using KdyWeb.Utility;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace KdyWeb.Service.ImageSave
{

    /// <summary>
    /// 图床关联 服务实现
    /// </summary>
    public class KdyImgSaveService : BaseKdyService, IKdyImgSaveService
    {
        private readonly IKdyImgSaveRepository _kdyImgSaveRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMinIoFileService _minIoFileService;
        private readonly IWeiBoFileService _weiBoFileService;
        private readonly INormalFileService _normalFileService;
        /// <summary>
        /// MinIO存储桶
        /// </summary>
        private const string bucketName = "kdyimg";

        public KdyImgSaveService(IKdyImgSaveRepository kdyImgSaveRepository, IMemoryCache memoryCache, IMinIoFileService minIoFileService,
            IWeiBoFileService weiBoFileService, INormalFileService normalFileService)
        {
            _kdyImgSaveRepository = kdyImgSaveRepository;
            _memoryCache = memoryCache;
            _minIoFileService = minIoFileService;
            _weiBoFileService = weiBoFileService;
            _normalFileService = normalFileService;
        }

        /// <summary>
        /// 通过Url上传
        /// </summary>
        /// <param name="imgUrl">图片Url</param>
        /// <returns></returns>
        public async Task<KdyResult<string>> PostFileByUrl(string imgUrl)
        {
            var ext = Path.GetExtension(imgUrl);
            var result = KdyResult.Error<string>(KdyResultCode.Error, "图片上传失败");

            var fileName = $"{DateTime.Now.Ticks:x}{ext}";
            //自有MinIo
            var minIoInput = new MinIoFileInput(bucketName, fileName, imgUrl);
            var minIoResult = await _minIoFileService.PostFile(minIoInput);
            if (minIoResult.IsSuccess == false)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, $"上传主通道失败，请稍后 {minIoResult.Msg}");
            }

            var dbImg = await _kdyImgSaveRepository.FirstOrDefaultAsync(a => a.FileMd5 == minIoResult.Data.FileMd5);
            if (dbImg != null)
            {
                result.Data = await GetImageByImgId(dbImg.Id);
                return result;
            }

            //微博
            var weiBoInput = new BaseKdyFileInput(imgUrl);
            var weiBoResult = await _weiBoFileService.PostFile(weiBoInput);

            //普通上传
            var normalInput = new NormalFileInput("https://niupic.com/index/upload/process", "image_field",
                "data", fileName, imgUrl);
            var normalResult = await _normalFileService.PostFile(normalInput);

            if (weiBoResult.IsSuccess)
            {
                dbImg = new KdyImgSave(minIoResult.Data.FileMd5, weiBoResult.Data.Url, minIoResult.Data.Url);
                if (normalResult.IsSuccess)
                {
                    dbImg.TwoUrl = normalResult.Data.Url;
                }
            }
            else if (normalResult.IsSuccess)
            {
                dbImg = new KdyImgSave(minIoResult.Data.FileMd5, normalResult.Data.Url, minIoResult.Data.Url);
            }
            else
            {
                return result;
            }

            await _kdyImgSaveRepository.CreateAsync(dbImg);

            return KdyResult.Success($"/kdyImg/path/{dbImg.Id}", "获取成功");
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
            var host = KdyConfiguration.GetValue<string>(KdyWebServiceConst.ImgHostKey);
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
                return UrlHandler(dbImg.MainUrl);
            }

            if (dbImg.OneUrl.IsEmptyExt() == false)
            {
                return UrlHandler(dbImg.OneUrl);
            }

            if (dbImg.TwoUrl.IsEmptyExt() == false)
            {
                return UrlHandler(dbImg.TwoUrl);
            }

            if (dbImg.Urls.Any())
            {
                return $"{host}/{dbImg.Urls.First().TrimStart('/')}";
            }

            return defaultUrl;
        }

        /// <summary>
        /// Url处理
        /// </summary>
        /// <returns></returns>
        private string UrlHandler(string url)
        {
            var host = KdyConfiguration.GetValue<string>(KdyWebServiceConst.ImgHostKey);

            if (url.StartsWith($"/{bucketName}"))
            {
                return $"{host}{url}";
            }

            return url;
        }
    }
}
