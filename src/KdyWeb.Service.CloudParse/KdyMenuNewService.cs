using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.KdyUserNew;
using KdyWeb.IRepository.KdyUserNew;
using KdyWeb.IService.CloudParse;
using Microsoft.EntityFrameworkCore;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 菜单 服务实现
    /// </summary>
    public class KdyMenuNewService : BaseKdyService, IKdyMenuNewService
    {
        private readonly IKdyMenuNewRepository _kdyMenuNewRepository;

        public KdyMenuNewService(IUnitOfWork unitOfWork,
            IKdyMenuNewRepository kdyMenuNewRepository) : base(unitOfWork)
        {
            _kdyMenuNewRepository = kdyMenuNewRepository;
        }

        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryPageMenuDto>>> QueryPageMenuAsync(QueryPageMenuInput input)
        {
            var query = _kdyMenuNewRepository.GetQuery();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.MenuName.Contains(input.KeyWord) ||
                                         a.MenuUrl.Contains(input.KeyWord) ||
                                         a.RouteName.Contains(input.KeyWord));
            }

            var dbList = await query
                .OrderByDescending(a => a.OrderBy)
                .ThenByDescending(a => a.CreatedTime)
                .ToListAsync();
            var tempDto = dbList.MapToListExt<QueryPageMenuDto>();
            var resultList = QueryPageMenuDto.GenerateMenuTree(tempDto);
            if (resultList == null ||
                resultList.Any() == false)
            {
                return KdyResult.Success(new PageList<QueryPageMenuDto>(input.Page, input.PageSize)
                {
                    Data = new List<QueryPageMenuDto>()
                });
            }

            var resultDto = KdyResult.Success(new PageList<QueryPageMenuDto>(input.Page, input.PageSize)
            {
                DataCount = resultList.Count,
                Data = resultList.Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToList()
            });
            return resultDto;
        }

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetAllMenuTreeDto>>> GetAllMenuTreeAsync()
        {
            var query = _kdyMenuNewRepository.GetQuery();
            var dbList = await query.ToListAsync();
            var tempDto = dbList.MapToListExt<GetAllMenuTreeDto>();
            var resultList = GetAllMenuTreeDto.GenerateMenuTree(tempDto);

            var resultDto = KdyResult.Success(resultList ?? new List<GetAllMenuTreeDto>());
            return resultDto;
        }

        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateMenuAsync(CreateAndUpdateMenuInput input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateMenuAsync(input);
            }

            return await CreateMenuAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchDeleteForLongKeyInput input)
        {
            //todo:只实现了单删除
            if (input.Ids == null || input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var firstId = input.Ids.First();
            var dbEntity = await _kdyMenuNewRepository.FirstOrDefaultAsync(a => a.Id == firstId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "删除失败，菜单不存在");
            }

            if (await _kdyMenuNewRepository.GetAsNoTracking().AnyAsync(a => a.ParentMenuId == dbEntity.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "删除失败，该菜单下存在子节点");
            }

            _kdyMenuNewRepository.Delete(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("删除成功");
        }

        /// <summary>
        /// 获取所有一级菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetAllOneMenuAsync()
        {
            var query = _kdyMenuNewRepository.GetQuery();
            query = query.Where(a => a.ParentMenuId == 0);

            var dbList = await query.ToListAsync();
            var tempDto = dbList.MapToListExt<GetRoleActivateMenuDto>();
            return KdyResult.Success(tempDto);
        }

        #region 私有
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> CreateMenuAsync(CreateAndUpdateMenuInput input)
        {
            if (await _kdyMenuNewRepository.RouteNameExistAsync(input.RouteName))
            {
                return KdyResult.Error(KdyResultCode.Error, "当前角色已存在此路由名");
            }

            var dbEntity = new KdyMenuNew(input.MenuUrl, input.MenuName, input.RouteName)
            {
                IconPrefix = input.IconPrefix,
                Icon = input.Icon,
                IsRootPath = input.IsRootPath ?? false,
                IsCache = input.IsCache ?? false,
                LocalFilePath = input.LocalFilePath,
                OrderBy = input.OrderBy
            };
            dbEntity.SetParentMenuId(input.ParentMenuId ?? 0);

            await _kdyMenuNewRepository.CreateAsync(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> UpdateMenuAsync(CreateAndUpdateMenuInput input)
        {
            var dbEntity = await _kdyMenuNewRepository.FirstOrDefaultAsync(a => a.Id == input.Id.Value);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误");
            }

            if (await _kdyMenuNewRepository.RouteNameExistAsync(input.Id.Value, input.RouteName))
            {
                return KdyResult.Error(KdyResultCode.Error, "当前角色已存在此路由名");
            }

            dbEntity.IconPrefix = input.IconPrefix;
            dbEntity.Icon = input.Icon;
            dbEntity.IsRootPath = input.IsRootPath ?? false;
            dbEntity.IsCache = input.IsCache ?? false;
            dbEntity.LocalFilePath = input.LocalFilePath;
            dbEntity.OrderBy = input.OrderBy;

            dbEntity.SetParentMenuId(input.ParentMenuId ?? 0);
            dbEntity.SetMenuUrl(input.MenuUrl);
            dbEntity.SetMenuName(input.MenuName);
            dbEntity.SetRouteName(input.RouteName);
            _kdyMenuNewRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
        #endregion

    }
}
