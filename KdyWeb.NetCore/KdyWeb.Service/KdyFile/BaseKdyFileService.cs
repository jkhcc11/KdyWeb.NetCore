using System;
using System.Net.Http;
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
        public virtual Task<KdyResult<KdyFileDto>> PostFile(T input)
        {
            if (input.FileBytes != null && input.FileBytes.Length > 0)
            {
                return PostFileByBytes(input);
            }

            if (string.IsNullOrEmpty(input.FileUrl) == false)
            {
                return PostFileByUrl(input);
            }

            throw new Exception($"文件上传失败 入参FileUrl和FileBytes必须二选一");
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
    }
}
