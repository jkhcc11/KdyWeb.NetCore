﻿using System;
using KdyWeb.BaseInterface.KdyRedis;
using KdyWeb.BaseInterface.LoginInfo;
using Microsoft.AspNetCore.Http;

namespace KdyWeb.BaseInterface.Service
{
    /// <summary>
    /// 登录信息 实现
    /// </summary>
    public class LoginUserInfo : ILoginUserInfo
    {
        private readonly IKdyRedisCache _kdyRedisCache;
        public LoginUserInfo(IHttpContextAccessor httpContextAccessor, IKdyRedisCache kdyRedisCache)
        {
            _kdyRedisCache = kdyRedisCache;
            if (httpContextAccessor?.HttpContext == null)
            {
                return;
            }

            InitUserInfo(httpContextAccessor.HttpContext);
        }

        public string UserAgent { get; set; }
        public string UserNick { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }

        /// <summary>
        /// 从当前请求初始化登录信息
        /// </summary>
        /// <param name="httpContext"></param>
        internal void InitUserInfo(HttpContext httpContext)
        {
            //todo:后期改成统一认证获取
            var request = httpContext.Request;
            if (request.Cookies.ContainsKey(KdyBaseConst.CookieKey) == false)
            {
                //无登录cookie
                return;
            }

            var cookie = request.Cookies[KdyBaseConst.CookieKey];
            if (string.IsNullOrEmpty(cookie))
            {
                //cookie为空
                return;
            }

            if (_kdyRedisCache == null)
            {
                throw new Exception($"{nameof(LoginUserInfo)} IKdyRedisCache为空");
            }

            var value = (string)_kdyRedisCache.GetDb(1).StringGet(cookie);
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            int.TryParse(cookie.Split('@')[1], out int userId);
            if (userId > 0)
            {
                UserId = userId;
            }
          
        }
    }
}