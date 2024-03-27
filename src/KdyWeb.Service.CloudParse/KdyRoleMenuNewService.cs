using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.BaseInterface.Service;
using KdyWeb.Dto.CloudParse;
using KdyWeb.Entity.KdyUserNew;
using KdyWeb.IRepository.KdyUserNew;
using KdyWeb.IService.CloudParse;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 角色菜单 服务实现
    /// </summary>
    public class KdyRoleMenuNewService : BaseKdyService, IKdyRoleMenuNewService
    {
        private readonly IKdyRoleMenuNewRepository _kdyRoleMenuNewRepository;


        public KdyRoleMenuNewService(IUnitOfWork unitOfWork, IKdyRoleMenuNewRepository kdyRoleMenuNewRepository) : base(unitOfWork)
        {
            _kdyRoleMenuNewRepository = kdyRoleMenuNewRepository;
        }

        /// <summary>
        /// 获取角色已激活菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetRoleActivateMenuAsync(string roleName)
        {
            var dbList = await _kdyRoleMenuNewRepository.GetActivatedMenuByRoleNameAsync(roleName);
            var tempDto = dbList.MapToListExt<GetRoleActivateMenuDto>();
            var resultDto = KdyResult.Success(tempDto);
            return resultDto;
        }

        /// <summary>
        /// 创建和更新角色菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateRoleMenuAsync(CreateAndUpdateRoleMenuInput input)
        {
            //1、当前db中角色所有菜单列表
            var dbRoleMenu = await _kdyRoleMenuNewRepository.GetAllMenuByRoleNameAsync(input.RoleName);
            if (dbRoleMenu.Any() == false &&
                input.MenuItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误，请至少选择一项");
            }

            #region 方案一 先删除 后新增

            if (dbRoleMenu.Any())
            {
                _kdyRoleMenuNewRepository.Delete(dbRoleMenu);
            }

            var createRoleMenu = input.MenuItems
                    .Select(a => new KdyRoleMenuNew(a.MenuId, input.RoleName, true))
                    .ToList();
            await _kdyRoleMenuNewRepository.CreateAsync(createRoleMenu);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
            #endregion

            #region 方案二 挨个更新

            //#region 全反选特殊情况

            //if (input.MenuItems.Any() == false &&
            //       dbRoleMenu.Any())
            //{
            //    //全部未激活
            //    foreach (var item in dbRoleMenu)
            //    {
            //        item.NotActivated();
            //    }

            //    _kdyRoleMenuNewRepository.Update(dbRoleMenu);
            //    await UnitOfWork.SaveChangesAsync();
            //    return KdyResult.Success();
            //}

            //#endregion

            //var updateRoleMenu = new List<KdyRoleMenuNew>();
            //var createRoleMenu = new List<KdyRoleMenuNew>();
            ////2、input菜单Id和db取差集，就是要新增的
            //if (input.MenuItems.Any())
            //{
            //    var dbMenuIds = dbRoleMenu.Select(a => a.MenuId).ToArray();
            //    var menuIds = input.MenuItems.Select(a => a.MenuId).ToArray();
            //    createRoleMenu = menuIds.Except(dbMenuIds)
            //        .Select(a => new KdyRoleMenuNew(a, input.RoleName, true))
            //        .ToList();
            //}

            ////3、遍历db中菜单Id,input存在则激活 否则未激活
            //foreach (var dbItem in dbRoleMenu)
            //{
            //    if (input.MenuItems.Any(a => a.MenuId == dbItem.MenuId))
            //    {
            //        dbItem.Activated();
            //        updateRoleMenu.Add(dbItem);
            //    }
            //    else
            //    {
            //        dbItem.NotActivated();
            //        updateRoleMenu.Add(dbItem);
            //    }
            //}

            ////4、保存
            //if (createRoleMenu.Any())
            //{
            //    await _kdyRoleMenuNewRepository.CreateAsync(createRoleMenu);
            //}

            //if (updateRoleMenu.Any())
            //{
            //    _kdyRoleMenuNewRepository.Update(createRoleMenu);
            //}

            //await UnitOfWork.SaveChangesAsync();
            //return KdyResult.Success(); 

            #endregion
        }

        /// <summary>
        /// 获取角色已激活菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetVueMenuWithWorkVueDto>>> GetRoleActivateMenuTreeAsync(string[] roleNames)
        {
            var dbList = await _kdyRoleMenuNewRepository.GetActivatedMenuByRoleNameAsync(roleNames);
            var tempDto = dbList.MapToListExt<GetVueMenuWithWorkVueDto>();
            return KdyResult.Success(GetVueMenuWithWorkVueDto.GenerateMenuTreeAndParentHandler(tempDto));
        }
    }
}
