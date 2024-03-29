﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.KdyFile;
using KdyWeb.Dto.KdyImg;
using KdyWeb.Entity;
using KdyWeb.IRepository.ImageSave;
using KdyWeb.IService.ImageSave;
using KdyWeb.IService.KdyFile;
using KdyWeb.Repository;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
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
        private static readonly Dictionary<string, List<byte[]>> FileSignature =
            new Dictionary<string, List<byte[]>>
            {
                { ".jpeg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    }
                },
                { ".jpg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    }
                },
                { ".png", new List<byte[]>
                    {
                        new byte[] { 0x89, 0x50 ,0x4E ,0x47,0x0D,0x0A,0x1A,0x0A }
                    }
                },
                { ".gif", new List<byte[]>
                    {
                        new byte[] { 0x47,0x49,0x46,0x38 }
                    }
                },
            };

        public KdyImgSaveService(IKdyImgSaveRepository kdyImgSaveRepository, IMemoryCache memoryCache, IMinIoFileService minIoFileService,
            IWeiBoFileService weiBoFileService, INormalFileService normalFileService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _kdyImgSaveRepository = kdyImgSaveRepository;
            _memoryCache = memoryCache;
            _minIoFileService = minIoFileService;
            _weiBoFileService = weiBoFileService;
            _normalFileService = normalFileService;

            CanUpdateFieldList.AddRange(new[]
            {
                nameof(KdyImgSave.FileMd5),
                nameof(KdyImgSave.MainUrl),
                nameof(KdyImgSave.OneUrl),
                nameof(KdyImgSave.TwoUrl),
                nameof(KdyImgSave.Urls),
            });
        }

        /// <summary>
        /// 分页查询图床
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryKdyImgDto>>> QueryKdyImgAsync(QueryKdyImgInput input)
        {
            var host = KdyConfiguration.GetValue<string>(KdyWebServiceConst.ImgHostKey);
            if (input.OrderBy == null || input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(KdyImgSave.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var result = await _kdyImgSaveRepository.GetAsNoTracking()
                .GetDtoPageListAsync<KdyImgSave, QueryKdyImgDto>(input);
            foreach (var item in result.Data)
            {
                item.FullImgUrl = $"{host}/kdyImg/path/{item.Id}";
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateValueByFieldAsync(UpdateValueByFieldInput input)
        {
            var dbImg = await _kdyImgSaveRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (dbImg == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "Id错误");
            }

            if (CanUpdateFieldList.Contains(input.Field) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, $"更新失败，当前字段：{input.Field} 不支持更新");
            }

            dbImg.UpdateModelField(input.Field, input.Value);

            _kdyImgSaveRepository.Update(dbImg);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除图床
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbImg = await _kdyImgSaveRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _kdyImgSaveRepository.Delete(dbImg);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success("图床删除成功");
        }

        /// <summary>
        /// 通过Url上传
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<string>> PostFileByUrl(PostFileByUrlInput input)
        {
            var ext = Path.GetExtension(input.ImgUrl);
            if (string.IsNullOrEmpty(ext))
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "文件后缀不能为空，仅支持.jpg,.jpeg,.png,.gif");
            }

            ext = ext.ToLower();
            if (FileSignature.ContainsKey(ext) == false)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "仅支持.jpg,.jpeg,.png,.gif");
            }

            //自有必传备用 微博成功则设置为主、否则备用设置为主 
            var fileName = $"{DateTime.Now.Ticks:x}{ext}";
            return await UploadMainAsync(fileName, input.ImgUrl);
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
            var url = GetImageForImgHandler(dbImg);
            _memoryCache.Set(cacheKey, url, TimeSpan.FromDays(1));

            return url;
        }

        /// <summary>
        /// 通过Byte上传
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<string>> PostFileByByteAsync(PostFileByByteInput input)
        {
            await using var memoryStream = new MemoryStream();
            await input.KdyFile.CopyToAsync(memoryStream);
            var maxSize = KdyConfiguration.GetValue<int>(KdyWebServiceConst.UploadConfig.UploadImgMaxSize);

            if (memoryStream.Length > maxSize * 1024 * 1024)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "请重新选择文件，超过5MB");
            }

            var ext = Path.GetExtension(input.KdyFile.FileName);
            //二进制文件
            var bytes = memoryStream.ToArray();
            var tempList = bytes.ToList();

            var signatures = FileSignature[ext];
            var maxHeaderSize = signatures.Max(m => m.Length);

            //文件头字节
            var headerBytes = tempList.Take(maxHeaderSize);
            var isCheck = signatures.Any(item =>
                headerBytes.Take(item.Length).SequenceEqual(item));
            if (isCheck == false)
            {
                return KdyResult.Error<string>(KdyResultCode.Error, "仅支持.jpg,.png,.gif");
            }

            //自有必传备用 微博成功则设置为主、否则备用设置为主 
            var fileName = $"{DateTime.Now.Ticks:x}{ext}";
            return await UploadMainAsync(fileName, bytes);
        }

        #region 私有
        /// <summary>
        /// 数据库图片处理
        /// </summary>
        /// <returns></returns>
        private string GetImageForImgHandler(KdyImgSave dbImg)
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
                return ResultUrlHandler(dbImg.MainUrl);
            }

            if (dbImg.OneUrl.IsEmptyExt() == false)
            {
                return ResultUrlHandler(dbImg.OneUrl);
            }

            if (dbImg.TwoUrl.IsEmptyExt() == false)
            {
                return ResultUrlHandler(dbImg.TwoUrl);
            }

            if (dbImg.Urls.Any())
            {
                return $"{host}/{dbImg.Urls.First().TrimStart('/')}";
            }

            return defaultUrl;
        }

        /// <summary>
        /// 原始图片Url处理
        /// </summary>
        /// <returns></returns>
        private string ResultUrlHandler(string originalUrl)
        {
            var host = KdyConfiguration.GetValue<string>(KdyWebServiceConst.ImgHostKey);
            var proxyHost = KdyConfiguration.GetValue<string>(KdyWebServiceConst.NgProxyKey);

            if (originalUrl.StartsWith($"/{bucketName}"))
            {
                return $"{host}{originalUrl}";
            }

            //http://pan-yz.chaoxing.com/thumbnail/origin/a37bb135f9192799960ca97c488747f7?type=img
            //=>//xxx.com/cximg/60c70132a7b45d8c902cb099add0ba7f.png
            if (originalUrl.Contains("pan-yz.chaoxing.com"))
            {
                //超星替换 否则403
                originalUrl = originalUrl.Replace("http://pan-yz.chaoxing.com/thumbnail/origin/", $"{proxyHost}/cximg/")
                    .Replace("?type=img", ".png");
            }

            if (originalUrl.Contains("pan-yz.chaoxing.com"))
            {
                //豆瓣替换 否则403
                //https://img1.doubanio.com/view/photo/s_ratio_poster/public/p2665925017.jpg
                //=>//xxx.com/dbimg/p2665925017.jpg
                originalUrl = originalUrl.Replace("https://img1.doubanio.com/view/photo/s_ratio_poster/public/", $"{proxyHost}/dbimg/");
            }
            return originalUrl;
        }

        /// <summary>
        /// 普通文件上传
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<KdyFileDto>> NormalUploadAsync(BaseKdyFileInput kdyFileInput)
        {
            //普通上传
            var normalInput = new NormalFileInput("https://niupic.com/index/upload/process", "image_field",
                "data", kdyFileInput.FileName);
            if (kdyFileInput.FileBytes != null && kdyFileInput.FileBytes.Any())
            {
                normalInput.SetFileBytes(kdyFileInput.FileBytes);
            }
            else if (string.IsNullOrEmpty(kdyFileInput.FileUrl) == false)
            {
                normalInput.SetFileUrl(kdyFileInput.FileUrl);
            }
            else
            {
                throw new KdyCustomException($"普通文件上传失败，无效上传数据。url和Bytes不能同时为空");
            }

            var result = await _normalFileService.PostFile(normalInput);
            if (result.IsSuccess)
            {
                return result;
            }

            //超星
            //var uid = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.UploadConfigCxPUid);
            //var token = KdyConfiguration.GetValue<string>(KdyWebServiceConst.UploadConfig.UploadConfigCxToken);
            //if (string.IsNullOrEmpty(uid) == false &&
            //    string.IsNullOrEmpty(token) == false)
            //{
            //    NormalFileInput normalInput = null;
            //    if (imgData is string imgUrl)
            //    {
            //        normalInput = new NormalFileInput("https://pan-yz.chaoxing.com/upload", "file",
            //            "data.thumbnail", fileName, imgUrl);
            //    }

            //    if (imgData is byte[] bytes)
            //    {
            //        normalInput = new NormalFileInput("https://pan-yz.chaoxing.com/upload", "file",
            //            "data.thumbnail", fileName, bytes);
            //    }

            //    if (normalInput == null)
            //    {
            //        throw new KdyCustomException($"普通文件上传失败，无效上传数据。{imgData.GetType()}");
            //    }

            //    normalInput.PostParDic = new Dictionary<string, string>()
            //    {
            //        {"id", "WU_FILE_0"},
            //        {"type", fileName.FileNameToContentType()},
            //        {"puid", uid},
            //        {"_token", token},
            //    };

            //    result = await _normalFileService.PostFile(normalInput);
            //}

            //if (result.IsSuccess)
            //{
            //    return result;
            //}

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

        /// <summary>
        /// 上传主通道
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<string>> UploadMainAsync(string fileName, string imgUrl)
        {
            var minIoInput = new MinIoFileInput(bucketName, fileName);
            var weiBoInput = new WeiBoFileInput();
            weiBoInput.SetFileUrl(imgUrl);
            minIoInput.SetFileUrl(imgUrl);

            return await UploadAsync(minIoInput, weiBoInput);
        }

        /// <summary>
        /// 上传主通道
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult<string>> UploadMainAsync(string fileName, byte[] imgData)
        {
            var minIoInput = new MinIoFileInput(bucketName, fileName);
            var weiBoInput = new WeiBoFileInput();
            minIoInput.SetFileBytes(imgData);
            weiBoInput.SetFileBytes(imgData);

            return await UploadAsync(minIoInput, weiBoInput);
        }

        internal async Task<KdyResult<string>> UploadAsync(MinIoFileInput minIoInput, WeiBoFileInput weiBoInput)
        {
            var host = KdyConfiguration.GetValue<string>(KdyWebServiceConst.ImgHostKey);
            var errDeafultId = KdyConfiguration.GetValue(KdyWebServiceConst.UploadImgErrDefaultId, "1139766229985267712");
            var result = KdyResult.Error<string>(KdyResultCode.Error, "图片上传失败");

            //1、上传主通道 得到md5
            var minIoResult = await _minIoFileService.PostFile(minIoInput);
            if (minIoResult.IsSuccess == false)
            {
                if (minIoResult.Code == KdyResultCode.HttpError)
                {
                    //http异常时 为默认图
                    return KdyResult.Success($"{host}/kdyImg/path/{errDeafultId}", "获取缺省图");
                }

                return KdyResult.Error<string>(KdyResultCode.Error, $"上传主通道失败，请稍后 {minIoResult.Msg}");
            }

            //2、md5是否存在
            var dbImg = await _kdyImgSaveRepository.FirstOrDefaultAsync(a => a.FileMd5 == minIoResult.Data.FileMd5);
            if (dbImg != null)
            {
                return KdyResult.Success($"{host}/kdyImg/path/{dbImg.Id}", "获取成功");
            }

            //3、微博上传
            var weiBoResult = await _weiBoFileService.PostFile(weiBoInput);

            //4、备用一个
            var normalResult = await NormalUploadAsync(minIoInput);
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
        #endregion
    }
}
