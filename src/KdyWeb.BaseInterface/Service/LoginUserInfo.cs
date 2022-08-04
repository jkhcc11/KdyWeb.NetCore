using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 登录信息 实现
    /// </summary>
    public class LoginUserInfo : ILoginUserInfo
    {
        public LoginUserInfo(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext == null)
            {
                return;
            }

            InitUserInfo(httpContextAccessor.HttpContext);
        }

        public bool IsLogin { get; set; }

        public string UserAgent { get; set; }

        public string UserNick { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public long? UserId { get; set; }

        public bool IsSuperAdmin { get; set; }

        public long GetUserId()
        {
            if (UserId.HasValue == false)
            {
                throw new KdyCustomException("用户信息丢失");
            }

            return UserId.Value;
        }

        public string LoginToken { get; set; }
        public string RoleName { get; set; }

        /// <summary>
        /// 从当前请求初始化登录信息
        /// </summary>
        internal void InitUserInfo(HttpContext httpContext)
        {
            var user = httpContext.User;
            if (user == null)
            {
                return;
            }

            if (httpContext.User.Identity.IsAuthenticated == false)
            {
                return;
            }

            var subStr = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrEmpty(subStr))
            {
                return;
            }

            var roleName = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.Role)?.Value;

            IsLogin = true;
            UserId = long.Parse(subStr);
            UserName = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.Name)?.Value;
            UserEmail = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.Email)?.Value;
            UserNick = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.NickName)?.Value;
            IsSuperAdmin = user.HasClaim(a => a.Type == JwtClaimTypes.Role &&
                                              a.Value == AuthorizationConst.NormalRoleName.SuperAdmin);
            LoginToken = httpContext.Request.Headers["Authorization"];
            RoleName = roleName;

            #region old

            ////old
            ////var request = httpContext.Request;
            ////if (request.Headers.ContainsKey(KdyBaseConst.OldApiAuthKey) == false)
            ////{
            ////    //无登录cookie
            ////    return;
            ////}

            ////var authKey = request.Headers[KdyBaseConst.OldApiAuthKey];
            ////if (string.IsNullOrEmpty(authKey))
            ////{
            ////    //cookie为空
            ////    return;
            ////}

            ////if (_kdyRedisCache == null)
            ////{
            ////    throw new Exception($"{nameof(LoginUserInfo)} IKdyRedisCache为空");
            ////}

            ////var value = (string)_kdyRedisCache.GetDb(1).StringGet($"parse:{authKey}");
            ////if (string.IsNullOrEmpty(value))
            ////{
            ////    return;
            ////}

            ////var temp = JsonConvert.DeserializeObject<LoginUserInfo>(value);
            ////UserId = temp.UserId;
            ////UserName = temp.UserName;
            ////UserEmail = temp.UserEmail;
            ////UserNick = temp.UserNick;
            ////IsSuperAdmin = temp.IsSuperAdmin;
            //////long.TryParse(cookie.Split('@')[1], out long userId);
            //////if (userId > 0)
            //////{
            //////    UserId = userId;
            //////}

            #endregion
        }
    }
}
