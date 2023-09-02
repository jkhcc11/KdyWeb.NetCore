using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Exceptionless;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.KdyFile;
using KdyWeb.IService.FileStore;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.Exceptions;

namespace KdyWeb.Service.FileStore
{
    /// <summary>
    /// MinIO存储 服务实现
    /// </summary>
    public class MinIoFileService : BaseKdyFileService<MinIoFileInput>, IMinIoFileService
    {
        private readonly IConfiguration _configuration;
        public MinIoFileService(IHttpClientFactory httpClientFactory, IConfiguration configuration) :
            base(httpClientFactory)
        {
            _configuration = configuration;
        }

        public override async Task<KdyResult<KdyFileDto>> PostFileByBytes(MinIoFileInput input)
        {
            if (input.FileName.StartsWith("/") == false)
            {
                //没有带路径默认传到公用
                input.SetFileName($"public/{DateTime.Now:yyyyMMdd}/{input.FileName}");
            }

            var minIoClient = GetMinIoClient();
            var contentType = input.FileName.FileNameToContentType();
            var result = KdyResult.Error<KdyFileDto>(KdyResultCode.Error, "上传失败");
            try
            {
                //检查存储通是否存在 不存在则创建
                bool found = await minIoClient.BucketExistsAsync(input.BucketName);
                if (found == false)
                {
                    await minIoClient.MakeBucketAsync(input.BucketName, input.Location);
                }

                await using var memoryStream = new MemoryStream(input.FileBytes);
                await minIoClient.PutObjectAsync(input.BucketName, input.FileName, memoryStream, memoryStream.Length, contentType);

                var uploadResult = new KdyFileDto()
                {
                    Url = $"/{input.BucketName}/{input.FileName}"
                };

                result = KdyResult.Success(uploadResult);

            }
            catch (MinioException e)
            {
                result.Msg = $"Minio上传异常【{e.Message}】";
                e.ToExceptionless()
                    .AddTags(nameof(MinIoFileService), nameof(PostFileByBytes))
                    .Submit();
            }

            KdyLog.LogTrace("MinIo上传结束.入参：{input} 结果：{result}", input, result);
            return result;
        }

        public MinioClient GetMinIoClient()
        {
            var config = _configuration
                .GetSection(KdyWebServiceConst.MinIoConfigKey)
                .Get<MinioConfig>();
            var client = new MinioClient(config.ServerUrl, config.AccessKey, config.SecretKey, "cn-249");
            if (config.IsSSL)
            {
                return client.WithSSL();
            }

            return client;
        }
    }
}
