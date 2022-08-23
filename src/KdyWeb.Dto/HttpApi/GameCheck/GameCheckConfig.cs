using System.Collections.Generic;

namespace KdyWeb.Dto.HttpApi.GameCheck
{
    /// <summary>
    /// 游戏检查配置
    /// </summary>
    public class GameCheckConfig
    {
        /// <summary>
        /// sign salt
        /// </summary>
        public string GenshinSalt { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public List<string> Uid { get; set; } = new List<string>();

        /// <summary>
        /// cookie
        /// </summary>
        public List<string> Cookie { get; set; } = new List<string>();
    }
}
