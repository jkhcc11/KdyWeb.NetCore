using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;
using KdyWeb.IService.KdyFile;
using Microsoft.AspNetCore.Http;

namespace KdyWeb.Service.KdyFile
{
    /// <summary>
    /// 文件存储 抽象基类
    /// </summary>
    /// <typeparam name="T">扩展参数</typeparam>
    public abstract class BaseKdyFileService<T> : BaseKdyService, IKdyFileService<T>
        where T : class, IBaseKdyFileInput
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        protected BaseKdyFileService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        public virtual async Task<KdyResult<KdyFileDto>> PostFile(T input)
        {
            var result = KdyResult.Error<KdyFileDto>(KdyResultCode.Error, "文件上传失败");
            if (input.FileBytes != null && input.FileBytes.Length > 0)
            {
                result = await PostFileByBytes(input);
            }
            else if (string.IsNullOrEmpty(input.FileUrl) == false)
            {
                result = await PostFileByUrl(input);
            }

            if (result.IsSuccess && input.FileBytes != null)
            {
                result.Data.FileMd5 = GetFileMd5(input.FileBytes);
            }

            return result;
        }

        /// <summary>
        /// 字节上传
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<KdyFileDto>> PostFileByBytes(T input);

        /// <summary>
        /// Url上传
        /// </summary>
        /// <returns></returns>
        public abstract Task<KdyResult<KdyFileDto>> PostFileByUrl(T input);

        /// <summary>
        /// Byte转Base64
        /// </summary>
        /// <returns></returns>
        protected virtual string ByteToBase64(byte[] fileBytes)
        {
            return Convert.ToBase64String(fileBytes);
        }

        /// <summary>
        /// 根据文件Url获取二进制数据
        /// </summary>
        /// <param name="url">文件Url</param>
        /// <returns></returns>
        protected virtual Task<byte[]> GetFileBytesByUrl(string url)
        {
            var guid = Guid.NewGuid().ToString("N");
            var httpClient = _httpClientFactory.CreateClient(guid);
            return httpClient.GetByteArrayAsync(url);
        }

        /// <summary>
        /// 检查上传直接 为空则抛出异常
        /// </summary>
        protected void CheckFileBytes(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length <= 0)
            {
                throw new Exception("上传字节内容为空");
            }
        }

        /// <summary>
        /// 文件名转ContentType 若文件名无后缀则是流类型
        /// </summary>
        /// <returns></returns>
        protected string FileNameToContentType(string fileName)
        {
            if (fileName.ToLower().EndsWith(".jpg") ||
                fileName.ToLower().EndsWith(".png"))
            {
                return "image/jpeg";
            }

            if (fileName.ToLower().EndsWith(".gif"))
            {
                return "image/gif";
            }

            if (fileName.ToLower().EndsWith(".webp"))
            {
                return "image/webp";
            }

            return "application/octet-stream";
        }

        /// <summary>
        /// 获取文件Md5
        /// </summary>
        /// <returns></returns>
        protected string GetFileMd5(byte[] fileBytes)
        {
            using var md5 = new MD5CryptoServiceProvider();
            //计算data字节数组的哈希值
            var md5Data = md5.ComputeHash(fileBytes);
            return md5Data.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0'));
        }
    }
}
