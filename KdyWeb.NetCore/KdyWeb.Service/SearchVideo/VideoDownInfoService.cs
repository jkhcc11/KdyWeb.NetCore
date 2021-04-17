using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 视频播放记录 服务实现 
    /// </summary>
    public class VideoDownInfoService : BaseKdyService, IVideoDownInfoService
    {
        private readonly IKdyRepository<VideoDownInfo, long> _videoDownInfoRepository;

        public VideoDownInfoService(IUnitOfWork unitOfWork, IKdyRepository<VideoDownInfo, long> videoDownInfoRepository) : base(unitOfWork)
        {
            _videoDownInfoRepository = videoDownInfoRepository;
        }

        /// <summary>
        /// 查询下载地址
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryVideoDownInfoDto>>> QueryVideoDownInfoAsync(QueryVideoDownInfoInput input)
        {
            if (input.OrderBy == null || input.OrderBy.Any() == false)
            {
                input.OrderBy = new List<KdyEfOrderConditions>()
                {
                    new KdyEfOrderConditions()
                    {
                        Key = nameof(VideoDownInfo.CreatedTime),
                        OrderBy = KdyEfOrderBy.Desc
                    }
                };
            }

            var query = _videoDownInfoRepository.GetAsNoTracking();
            var result = await query
                .GetDtoPageListAsync<VideoDownInfo, QueryVideoDownInfoDto>(input);
            return KdyResult.Success(result);
        }
    }
}
