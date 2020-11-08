using System;
using Newtonsoft.Json;

namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 创建和修改人信息Dto
    /// </summary>
    public class CreatedUserDto<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(JsonConverterLong))]
        public TKey Id { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int? CreatedUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public int? ModifyUserId { get; set; }

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
