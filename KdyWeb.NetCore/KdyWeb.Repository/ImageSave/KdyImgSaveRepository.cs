using KdyWeb.BaseInterface.Repository;
using KdyWeb.Entity;
using KdyWeb.IRepository.ImageSave;

namespace KdyWeb.Repository.ImageSave
{
    /// <summary>
    /// 图床关联 仓储实现
    /// </summary>
    public class KdyImgSaveRepository : KdyRepository<KdyImgSave, long>, IKdyImgSaveRepository
    {
        public KdyImgSaveRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
