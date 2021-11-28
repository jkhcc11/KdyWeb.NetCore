using System;
using System.Linq;
using KdyWeb.BaseInterface;

namespace KdyWeb.CloudParse.Input
{
    /// <summary>
    /// 基础配置输入
    /// </summary>
    public class BaseConfigInput : IBaseConfigEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="reqUserInfo">Url用户信息</param>
        /// <param name="parseCookie">解析Cookie</param>
        public BaseConfigInput(long userId, string reqUserInfo, string parseCookie)
        {
            if (string.IsNullOrEmpty(reqUserInfo) ||
                reqUserInfo.Contains("_") == false)
            {
                //无子账号
                throw new KdyCustomException($"未找到子账号编号，{nameof(ReqUserInfo)}");
            }

            UserId = userId;
            ReqUserInfo = reqUserInfo;
            ParseCookie = parseCookie;
        }

        public string ReqUserInfo { get; set; }

        public long UserId { get; set; }

        public string ParseCookie { get; set; }

        /// <summary>
        /// 子账号Id
        /// </summary>
        public int ChildUserId => Convert.ToInt32(ReqUserInfo.Split('_')[1]);
    }
}
