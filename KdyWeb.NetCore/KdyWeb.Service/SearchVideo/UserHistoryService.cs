using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.SearchVideo;
using KdyWeb.Entity.SearchVideo;
using KdyWeb.IService.SearchVideo;

namespace KdyWeb.Service.SearchVideo
{
    /// <summary>
    /// 用户播放记录 服务实现 
    /// </summary>
    public class UserHistoryService : BaseKdyService, IUserHistoryService
    {
        private readonly IKdyRepository<UserHistory, long> _userHistoryRepository;

        public UserHistoryService(IKdyRepository<UserHistory, long> userHistoryRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userHistoryRepository = userHistoryRepository;
        }

        public async Task<KdyResult> CreateUserHistoryAsync(CreateUserHistoryInput input)
        {
            var dbInput = input.MapToExt<UserHistory>();

            CreateUserHistoryHandler(dbInput, input);
            await _userHistoryRepository.CreateAsync(dbInput);
            await UnitOfWork.SaveChangesAsync();

            return KdyResult.Success();
        }

        internal void CreateUserHistoryHandler(UserHistory dbUserHistory, CreateUserHistoryInput input)
        {
            dbUserHistory.EpId = input.EpId;
            dbUserHistory.VodUrl = input.VodUrl;
            dbUserHistory.KeyId = 1;
            dbUserHistory.VodName = "测试名称";
            dbUserHistory.EpName = "极速";
            dbUserHistory.UserName = LoginUserInfo.UserName;
        }
    }
}
