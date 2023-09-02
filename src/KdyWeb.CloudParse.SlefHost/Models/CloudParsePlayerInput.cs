using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParse.SelfHost.Models
{
    public class CloudParsePlayerInput
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [FromRoute]
        public string UserInfo { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        [FromRoute]
        public string FileInfo { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        [EnumDataType(typeof(ParseModel))]
        [FromQuery(Name = "model")]
        public ParseModel ParseModel { get; set; } = ParseModel.Name;

        [FromQuery(Name = "pt")]
        [EnumDataType(typeof(PlayerType))]
        public PlayerType PlayerType { get; set; } = PlayerType.DPlayer;

        /// <summary>
        /// 是否为旧版本模式 eg:  xxx_111
        /// </summary>
        public bool IsOldUserInfo => UserInfo.Contains("_");

        /// <summary>
        /// Api时使用
        /// </summary>
        [FromQuery(Name = "token")]
        public string Token { get; set; }
    }

    /// <summary>
    /// 解析模式
    /// </summary>
    public enum ParseModel
    {
        /// <summary>
        /// 文件Id 直链
        /// </summary>
        Id = 1,

        /// <summary>
        /// 文件Id 切片
        /// </summary>
        IdTs = 2,

        /// <summary>
        /// 文件Id 自动
        /// </summary>
        /// <remarks>
        /// mp4 直链 非mp4切片(如果支持)
        /// </remarks>
        IdAuto = 2,

        /// <summary>
        /// 名称 直链
        /// </summary>
        Name = 10,

        /// <summary>
        /// 名称 切片
        /// </summary>
        NameTs = 11,

        /// <summary>
        /// 名称 自动
        /// </summary>
        /// <remarks>
        /// mp4 直链 非mp4切片(如果支持)
        /// </remarks>
        NameAuto = 12
    }

    /// <summary>
    /// 播放器类型
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        /// DPlayer播放器
        /// </summary>
        DPlayer = 1,

        /// <summary>
        /// 直接返回
        /// </summary>
        Jump301,
    }
}
