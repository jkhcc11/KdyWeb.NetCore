using KdyWeb.BaseInterface.Service;
using KdyWeb.IService.CloudParse;
using System.Threading.Tasks;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.BaseInterface.Extensions;
using KdyWeb.BaseInterface.Repository;
using KdyWeb.Dto.CloudParse.KdyUserNew;
using KdyWeb.Entity.KdyUserNew;
using Microsoft.EntityFrameworkCore;
using KdyWeb.IRepository.KdyUserNew;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 角色 服务实现
    /// </summary>
    public class KdyRoleNewService : BaseKdyService, IKdyRoleNewService
    {
        private readonly IKdyRoleNewRepository _kdyRoleNewRepository;

        public KdyRoleNewService(IUnitOfWork unitOfWork,
            IKdyRoleNewRepository kdyRoleNewRepository)
            : base(unitOfWork)
        {
            _kdyRoleNewRepository = kdyRoleNewRepository;
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<PageList<QueryPageRoleDto>>> QueryPageRoleAsync(QueryPageRoleInput input)
        {
            var pageResult = await _kdyRoleNewRepository.GetQuery()
                .GetDtoPageListAsync<KdyRoleNew, QueryPageRoleDto>(input);
            return KdyResult.Success(pageResult);
        }

        /// <summary>
        /// 创建和更新角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateRoleAsync(input);
            }

            return await CreateRoleAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(long roleId)
        {
            var dbEntity = await _kdyRoleNewRepository.FirstOrDefaultAsync(a => a.Id == roleId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "删除失败，角色不存在");
            }

            _kdyRoleNewRepository.Delete(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success("删除成功");
        }

        #region 私有
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> CreateRoleAsync(CreateAndUpdateRoleInput input)
        {
            if (await _kdyRoleNewRepository.RoleFlagExistAsync(input.RoleFlag))
            {
                return KdyResult.Error(KdyResultCode.Error, "当前角色已存在");
            }

            var dbEntity = new KdyRoleNew(input.RoleName, input.RoleFlag)
            {
                RoleRemark = input.RoleRemark
            };

            await _kdyRoleNewRepository.CreateAsync(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> UpdateRoleAsync(CreateAndUpdateRoleInput input)
        {
            var dbEntity = await _kdyRoleNewRepository.FirstOrDefaultAsync(a => a.Id == input.Id.Value);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误");
            }

            if (await _kdyRoleNewRepository.RoleFlagExistAsync(input.Id.Value, input.RoleFlag))
            {
                return KdyResult.Error(KdyResultCode.Error, "当前角色已存在");
            }

            dbEntity.RoleRemark = input.RoleRemark;
            dbEntity.RoleFlag = input.RoleFlag;
            dbEntity.RoleName = input.RoleName;
            _kdyRoleNewRepository.Update(dbEntity);
            await UnitOfWork.SaveChangesAsync();
            return KdyResult.Success();
        }
        #endregion
    }
}
