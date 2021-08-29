using System;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 禁用日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableKdyLogAttribute : Attribute
    {

    }
}
