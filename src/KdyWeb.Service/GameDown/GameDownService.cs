using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.GameDown;
using KdyWeb.Entity.GameDown;
using KdyWeb.IService.GameDown;
using KdyWeb.Repository;

namespace KdyWeb.Service.GameDown
{
    /// <summary>
    /// 游戏下载资源 服务
    /// </summary>
    public class GameDownService : BaseKdyService, IGameDownService
    {
        private readonly IKdyRepository<GameInfoMain, long> _gameInfoRepository;
        public GameDownService(IUnitOfWork unitOfWork, IKdyRepository<GameInfoMain, long> gameInfoRepository)
            : base(unitOfWork)
        {
            _gameInfoRepository = gameInfoRepository;
        }

        /// <summary>
        /// 查询游戏下载列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryGameDownListWithAdminDto>>> QueryGameDownListWithAdminAsync(QueryGameDownListWithAdminInput input)
        {
            var result = await _gameInfoRepository.GetAsNoTracking()
                .GetDtoPageListAsync<GameInfoMain, QueryGameDownListWithAdminDto>(input);

            return KdyResult.Success(result);
        }
    }
}
