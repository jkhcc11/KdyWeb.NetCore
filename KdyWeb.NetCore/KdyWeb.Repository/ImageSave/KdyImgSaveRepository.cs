using System.Threading.Tasks;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity;
using KdyWeb.IRepository.ImageSave;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Repository.ImageSave
{
    /// <summary>
    /// 图床关联 仓储实现
    /// </summary>
    public class KdyImgSaveRepository : KdyRepository<KdyImgSave, long>, IKdyImgSaveRepository
    {
        private readonly IIdGenerate<long> _idGenerate;
        public KdyImgSaveRepository(DbContext dbContext, IIdGenerate<long> idGenerate) : base(dbContext)
        {
            _idGenerate = idGenerate;
        }

        public override async Task<KdyImgSave> CreateAsync(KdyImgSave entity)
        {
            entity.Id = _idGenerate.Create();
            return await base.CreateAsync(entity);
        }
    }
}
