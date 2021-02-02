using System;
using System.Runtime.Serialization;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class KdyCustomException : SystemException, ISerializable
    {
        public KdyCustomException(string msg) : base(msg)
        {

        }

        public KdyCustomException(string msg, Exception inner) : base(msg, inner)
        {

        }
    }
}
