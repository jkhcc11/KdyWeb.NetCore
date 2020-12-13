using System;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// Dto基类
    /// </summary>
    /// <remarks>
    ///  Id 
    /// </remarks>
    public class BaseEntityDto<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public TKey Id { get; set; }
    }

    /// <summary>
    /// 创建和修改人信息Dto
    /// </summary>
    /// <remarks>
    ///  CreatedUserId、 ModifyUserId、CreatedTime、ModifyTime
    /// </remarks>
    public class CreatedUserDto<TKey> : BaseEntityDto<TKey>
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long? CreatedUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public long? ModifyUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
