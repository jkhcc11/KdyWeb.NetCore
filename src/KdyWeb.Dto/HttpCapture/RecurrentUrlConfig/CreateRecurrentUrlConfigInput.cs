using AutoMapper;
using KdyWeb.Entity.HttpCapture;

namespace KdyWeb.Dto.HttpCapture
{
    /// <summary>
    /// 创建循环Url配置 Input
    /// </summary>
    [AutoMap(typeof(RecurrentUrlConfig), ReverseMap = true)]
    public class CreateRecurrentUrlConfigInput : BaseRecurrentUrlConfig
    {

    }
}
