using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.KdyFile;
using KdyWeb.IService.FileStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KdyWeb.Service.FileStore
{
    /// <summary>
    /// 文件存储 抽象基类
    /// </summary>
    /// <typeparam name="T">扩展参数</typeparam>
    public abstract class BaseKdyFileService<T> : IKdyService, IKdyFileService<T>
        where T : class, IBaseKdyFileInput
    {
        /// <summary>
        /// 统一日志
        /// </summary>
        protected readonly ILogger KdyLog;
        /// <summary>
        /// 统一配置
        /// </summary>
        protected readonly IConfiguration KdyConfiguration;

        protected readonly IHttpClientFactory _httpClientFactory;

        protected BaseKdyFileService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            KdyLog = KdyBaseServiceProvider.ServiceProvide.GetService<ILoggerFactory>().CreateLogger(GetType());
            KdyConfiguration = KdyBaseServiceProvider.ServiceProvide.GetRequiredService<IConfiguration>();
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
        protected async Task<KdyResult<KdyFileDto>> PostFileByUrl(T input)
        {
            try
            {
                var bytes = await GetFileBytesByUrl(input.FileUrl);
                input.SetFileBytes(bytes);
                return await PostFileByBytes(input);
            }
            catch (HttpRequestException httpEx)
            {
                httpEx.ToExceptionless()
                    .AddTags(nameof(BaseKdyFileService<T>), nameof(PostFileByUrl))
                    .Submit();
                return KdyResult.Error<KdyFileDto>(KdyResultCode.HttpError, $"Url上传失败源：【{httpEx.Message}】");
            }
            catch (Exception ex)
            {
                ex.ToExceptionless()
                    .AddTags(nameof(BaseKdyFileService<T>), nameof(PostFileByUrl))
                    .Submit();
                return KdyResult.Error<KdyFileDto>(KdyResultCode.Error, $"Url上传异常【{ex.Message}】");
            }
        }

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
            httpClient.DefaultRequestHeaders.Referrer = new Uri(url);
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
        /// 获取文件Md5
        /// </summary>
        /// <returns></returns>
        protected string GetFileMd5(byte[] fileBytes)
        {
            //using var md5 = new MD5CryptoServiceProvider();
            using var md5 = MD5.Create();
            //计算data字节数组的哈希值
            var md5Data = md5.ComputeHash(fileBytes);
            return md5Data.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0'));
        }

        public void Dispose()
        {
        }
    }
}
