using System;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Entity.CloudParse.Enum;
using KdyWeb.IService.CloudParse;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// ServerCookie 服务实现
    /// </summary>
    public class ServerCookieService : BaseKdyService, IServerCookieService
    {
        private readonly IKdyRepository<ServerCookie, long> _serverCookieRepository;
        private readonly IKdyRepository<CloudParseUserChildren, long> _cloudParseUserChildrenRepository;
        public ServerCookieService(IUnitOfWork unitOfWork, IKdyRepository<ServerCookie, long> serverCookieRepository,
            IKdyRepository<CloudParseUserChildren, long> cloudParseUserChildrenRepository)
            : base(unitOfWork)
        {
            _serverCookieRepository = serverCookieRepository;
            _cloudParseUserChildrenRepository = cloudParseUserChildrenRepository;
        }

        /// <summary>
        /// 查询服务器Cookie列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryServerCookieDto>>> QueryServerCookieAsync(QueryServerCookieInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var query = _serverCookieRepository.GetQuery();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CreatedUserId == userId);
            }

            var result = await query.GetDtoPageListAsync<ServerCookie, QueryServerCookieDto>(input);
            if (result.DataCount == 0)
            {
                return KdyResult.Success(result);
            }

            var subAccountList = await _cloudParseUserChildrenRepository.GetListAsync(a => a.IsDelete == false);
            foreach (var dtoItem in result.Data)
            {
                dtoItem.SubAccountAlias = subAccountList.FirstOrDefault(a => a.Id == dtoItem.SubAccountId)?.Alias;
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建和更新服务器Cookie
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateServerCookieAsync(CreateAndUpdateServerCookieInput input)
        {
            var userId = LoginUserInfo.GetUserId();
            var serverCookieQuery = _serverCookieRepository.GetQuery();
            serverCookieQuery = serverCookieQuery.Where(a => a.SubAccountId == input.SubAccountId &&
                                                             a.ServerIp == input.ServerIp);

            if (input.Id.HasValue)
            {
                serverCookieQuery = serverCookieQuery.Where(a => a.Id != input.Id);
                if (await serverCookieQuery.AnyAsync())
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败,子账号对应的服务器Ip已存在");
                }

                //修改
                var dbEntity = await _serverCookieRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
                if (dbEntity == null)
                {
                    return KdyResult.Error(KdyResultCode.Error, "修改失败,暂无信息");
                }

                var cacheKey = GetServerCookieCacheKey(dbEntity.SubAccountId);
                dbEntity.SubAccountId = input.SubAccountId;
                dbEntity.ServerIp = input.ServerIp;
                dbEntity.CookieInfo = input.CookieInfo;
                _serverCookieRepository.Update(dbEntity);
                await ClearCacheValueAsync(cacheKey);
            }
            else
            {
                if (await serverCookieQuery.AnyAsync())
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败,子账号对应的服务器Ip已存在");
                }

                //新增
                var dbEntity = new ServerCookie(input.SubAccountId, input.ServerIp, input.CookieInfo);
                await _serverCookieRepository.CreateAsync(dbEntity);
            }

            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input)
        {
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var dbEntities = await _serverCookieRepository.GetQuery()
                .Where(a => input.Ids.Contains(a.Id))
                .ToListAsync();
            _serverCookieRepository.Delete(dbEntities);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("删除成功");
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> AuditAsync(long id)
        {
            var dbEntity = await _serverCookieRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误");
            }

            dbEntity.ServerCookieStatus = ServerCookieStatus.Normal;
            _serverCookieRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取服务器Id缓存
        /// </summary>
        /// <param name="serverIp">服务器Ip</param>
        /// <param name="subAccountId">子账号Id</param>
        /// <returns></returns>
        public async Task<QueryServerCookieDto> GetServerCookieCacheAsync(string serverIp, long subAccountId)
        {
            var cacheKey = GetServerCookieCacheKey(subAccountId);
            var cacheValue = await GetCacheValueAsync(cacheKey, async () =>
            {
                var serverCookie = await _serverCookieRepository
                    .FirstOrDefaultAsync(a => a.SubAccountId == subAccountId &&
                                              a.ServerIp == serverIp &&
                                              a.ServerCookieStatus == ServerCookieStatus.Normal);
                var result = serverCookie.MapToExt<QueryServerCookieDto>();
                return result;
            }, TimeSpan.FromHours(12));

            return cacheValue;
        }

        /// <summary>
        /// 获取服务器缓存Key
        /// </summary>
        /// <returns></returns>
        private string GetServerCookieCacheKey(long subAccountId)
        {
            return $"{CacheKeyConst.KdyCacheName.ServerCookieKey}:{subAccountId}";
        }
    }
}
