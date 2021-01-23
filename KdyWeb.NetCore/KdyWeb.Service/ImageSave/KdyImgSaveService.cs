using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
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
    /// todo:迁移完新增七牛、阿里云oss等对象存储
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
            IWeiBoFileService weiBoFileService, INormalFileService normalFileService, IUnitOfWork unitOfWork) : base(unitOfWork)
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
            var host = KdyConfiguration.GetValue<string>(KdyWebServiceConst.ImgHostKey);
            var ext = Path.GetExtension(imgUrl);
            var result = KdyResult.Error<string>(KdyResultCode.Error, "图片上传失败");

            //自有必传备用 微博成功则设置为主、否则备用设置为主 
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
                return KdyResult.Success($"{host}/kdyImg/path/{dbImg.Id}", "获取成功");
            }

            //微博
            var weiBoInput = new BaseKdyFileInput(imgUrl);
           // var weiBoResult = KdyResult.Error<KdyFileDto>(KdyResultCode.Error, "待调整");
            var weiBoResult = await _weiBoFileService.PostFile(weiBoInput);

            var normalResult = await NormalUpload(fileName, imgUrl);
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
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success($"{host}/kdyImg/path/{dbImg.Id}", "获取成功");
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

        /// <summary>
        /// 普通文件上传
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<KdyFileDto>> NormalUpload(string fileName, string imgUrl)
        {
            var result = KdyResult.Error<KdyFileDto>(KdyResultCode.Error, "上传失败-1");

            //普通上传
            //var normalInput = new NormalFileInput("https://niupic.com/index/upload/process", "image_field",
            //    "data", fileName, imgUrl);
            //var normalResult = await _normalFileService.PostFile(normalInput);

            //超星
            var uid = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.UploadConfigCxPUid);
            var token = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.UploadConfigCxToken);
            if (string.IsNullOrEmpty(uid) == false &&
                string.IsNullOrEmpty(token) == false)
            {
                var normalInput = new NormalFileInput("https://pan-yz.chaoxing.com/upload", "file",
                    "data.thumbnail", fileName, imgUrl)
                {
                    PostParDic = new Dictionary<string, string>()
                    {
                        {"id","WU_FILE_0"},
                        {"type",fileName.FileNameToContentType()},
                        {"puid",uid},
                        {"_token",token},
                    }
                };
                result = await _normalFileService.PostFile(normalInput);
            }

            if (result.IsSuccess)
            {
                return result;
            }

            //腾讯文档 防盗 不能直接使用
            //var cookie = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.UploadConfigTxDocCookie);
            //var id = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.UploadConfigTxDocId);
            //if (string.IsNullOrEmpty(cookie) == false &&
            //    string.IsNullOrEmpty(id) == false)
            //{
            //    var normalInput = new NormalFileInput($"https://docs.qq.com/v1/image/upload?globalPadId={id}", "file",
            //        "url", fileName, imgUrl)
            //    {
            //        Referer = "https://docs.qq.com/doc/Ber22u2Q7NEp16bFF94wEJ9i1Em3pq3bVlYN4IQmKC2Cjyb92t3Kd93F0NWs3tprNM2iBB6w",
            //        Cookie = cookie
            //    };
            //    result = await _normalFileService.PostFile(normalInput);
            //    if (result.IsSuccess)
            //    {
            //        result.Data.Url = result.Data.Url.GetStrMathExt(":", "\\?w\\=");
            //    }
            //}

            return result;
        }
    }
}
