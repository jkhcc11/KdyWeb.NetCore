using AutoMapper;

namespace KdyWeb.Dto
{
    /// <summary>
    /// AutoMapper初始化 获取当前程序集
    /// </summary>
    /// <remarks>
    ///  1、若默认方式直接用使用特性Attribute标签方式 使用自带的特性方法
    ///     AutoMap 默认是 当前标记类为 输出 一般用于DB->DTO
    ///     AutoMap ReverseMap反向映射 当前标记类为 输入  一般用于Input->DB
    ///  2、若用了Profile附加时 (不会生效得去掉AutoMap特性)
    ///     Input=>DB 属性不一致时得使用ReverseMap
    /// </remarks>
    public class KdyMapperInit : Profile
    {
        public KdyMapperInit()
        {
            // CreateMap<User, SearchUserDto>();
            // CreateMap<Log, LogDto>();
        }
    }
}
