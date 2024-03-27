using AutoMapper;
using AutoMapper.Configuration.Annotations;
using KdyWeb.BaseInterface;
using KdyWeb.Entity.KdyUserNew;
using Newtonsoft.Json;

namespace KdyWeb.Dto.CloudParse
{
    /// <summary>
    /// 获取角色已激活菜单
    /// </summary>
    [AutoMap(typeof(KdyMenuNew))]
    public class GetRoleActivateMenuDto
    {
        /// <summary>
        /// 父菜单Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long ParentMenuId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        [SourceMember(nameof(KdyMenuNew.Id))]
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
    }
}
