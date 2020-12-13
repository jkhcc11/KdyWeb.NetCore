using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 视频弹幕 服务接口
    /// todo:调整为websocket
    /// </summary>
    public class VideoDanMuService : BaseKdyService, IVideoDanMuService
    {
        private readonly IKdyRepository<VideoDanMu, long> _videoDanMuRepository;

        public VideoDanMuService(IUnitOfWork unitOfWork, IKdyRepository<VideoDanMu, long> videoDanMuRepository) :
            base(unitOfWork)
        {
            _videoDanMuRepository = videoDanMuRepository;
        }

        /// <summary>
        /// 创建弹幕
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateDanMuAsync(CreateDanMuInput input)
        {
            var dbInput = input.MapToExt<VideoDanMu>();
            await _videoDanMuRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取视频剧集弹幕
        /// </summary>
        /// <param name="epId">剧集Id</param>
        /// <returns></returns>
        public async Task<KdyResult<string>> GetVideoDanMuAsync(long epId)
        {
            var dbList = await _videoDanMuRepository.GetAsNoTracking()
                .Where(a => a.EpId == epId)
                .OrderBy(a => a.DTime)
                .ToListAsync();

            var dm = new StringBuilder();
            dm.Append("<i>");
            foreach (var item in dbList)
            {
                var unix = item.CreatedTime.ToSecondTimestamp();
                //时间节点，模式，字体大小，颜色，时间戳，stime,用户名，时间戳
                dm.AppendFormat($"<d p=\"{item.DTime},{item.DMode},{item.DSize},{item.DColor},{unix},0,游客,{unix}\">{item.Msg}</d>");
            }
            dm.Append("</i>");

            return KdyResult.Success(dm.ToString(), "获取成功");
        }
    }
}
