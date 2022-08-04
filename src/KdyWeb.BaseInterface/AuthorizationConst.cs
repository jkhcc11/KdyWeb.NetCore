﻿namespace KdyWeb.BaseInterface
{

    /// <summary>
    /// 授权常量
    /// </summary>
    public class AuthorizationConst
    {
        /// <summary>
        /// 通用角色
        /// </summary>
        public class NormalRoleName
        {
            /// <summary>
            /// 超管
            /// </summary>
            public const string SuperAdmin = "kdyadmin";

            /// <summary>
            /// 普通
            /// </summary>
            public const string Normal = "normal";
        }

        /// <summary>
        /// 通用策略名
        /// </summary>
        public class NormalPolicyName
        {
            /// <summary>
            /// 超管策略名
            /// </summary>
            public const string SuperAdminPolicy = "Kdy_SuperAdmin_Policy";

            /// <summary>
            /// 普通策略名
            /// </summary>
            /// <remarks>
            ///  普通用户或管理员皆可
            /// </remarks>
            public const string NormalPolicy = "Kdy_Normal_Policy";

            /// <summary>
            /// 跨域防伪策略
            /// </summary>
            public const string CrossPolicy = "Kdy_Cross_Policy";
        }
    }
}