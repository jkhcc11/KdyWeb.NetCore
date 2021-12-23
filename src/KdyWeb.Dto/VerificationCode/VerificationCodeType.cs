using KdyWeb.BaseInterface;

namespace KdyWeb.Dto.VerificationCode
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerificationCodeType
    {
        /// <summary>
        /// 注册验证码
        /// </summary>
        Register = 1,

        /// <summary>
        /// 修改密码验证码
        /// </summary>
        ModifyPwd = 2,

        /// <summary>
        /// 找回密码
        /// </summary>
        FindPwd = 3
    }

    public static class VerificationCodeTypeExtension
    {
        /// <summary>
        /// 验证码缓存Key
        /// </summary>
        /// <returns></returns>
        public static string GetVerificationCodeCacheKey(this VerificationCodeType verificationCodeType, string email)
        {
            return $"{email}_{verificationCodeType}";
        }

        /// <summary>
        /// 根据验证码类型获取邮箱主题
        /// </summary>
        /// <returns></returns>
        public static string GetVerificationCodeEmailSubject(this VerificationCodeType codeType)
        {
            if (codeType == VerificationCodeType.Register)
            {
                return "看电影--用户注册验证码";
            }

            if (codeType == VerificationCodeType.ModifyPwd)
            {
                return "看电影--修改密码验证";
            }

            if (codeType == VerificationCodeType.FindPwd)
            {
                return "看电影--找回验证码";
            }

            throw new KdyCustomException("未知邮箱类型");
        }

        /// <summary>
        /// 根据验证码类型获取邮箱内容
        /// </summary>
        /// <returns></returns>
        public static string GetVerificationCodeEmailContent(this VerificationCodeType codeType, string code)
        {
            if (codeType == VerificationCodeType.Register)
            {
                return $"您正在注册账号，验证码是：{code}。切勿泄露给他人";
            }

            if (codeType == VerificationCodeType.ModifyPwd)
            {
                return $"您正在修改账号密码，验证码是：{code}。切勿泄露给他人";
            }

            if (codeType == VerificationCodeType.FindPwd)
            {
                return $"您正在找回密码，验证码是：{code}。切勿泄露给他人";
            }

            throw new KdyCustomException("未知邮箱类型");
        }
    }
}
