using System;
using System.Collections.Generic;
using System.Text;

namespace KdyWeb.Dto.HttpApi.GameCheck.GenShin
{
    /// <summary>
    /// GenShin 返回结构
    /// </summary>
    /// <typeparam name="TData">TData数据</typeparam>
    public class GenShinResult<TData>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回码
        /// </summary>
        public int RetCode { get; set; }

        public bool IsSuccess => RetCode == 0;
    }
}
