using AutoMapper;

namespace KdyWeb.Dto
{
    /// <summary>
    /// AutoMapper初始化 获取当前程序集
    /// 待封装为属性标签方式
    /// 使用特性Attribute标签方式 使用自带的特性方法
    /// AutoMap 默认是 当前标记类为 输出 一般用于DB->DTO
    /// AutoMap ReverseMap反向映射 当前标记类为 输入  一般用于Input->DB
    /// </summary>
    public class KdyMapperInit : Profile
    {
        public KdyMapperInit()
        {
            // CreateMap<User, SearchUserDto>();
            // CreateMap<Log, LogDto>();
        }
    }
}
