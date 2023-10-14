using System.ComponentModel.DataAnnotations;

namespace KdyWeb.Dto.Selenium
{
    /// <summary>
    /// Selenium模拟登录 Input
    /// </summary>
    public class LoginBySeleniumInput
    {
        /// <summary>
        /// 登录地址
        /// </summary>
        [Required(ErrorMessage = "请输入登录地址")]
        public string LoginUrl { get; set; } = "https://cloud.189.cn";

        /// <summary>
        /// 必要的Cookie值 多个用逗号隔开
        /// </summary>
        [Required(ErrorMessage = "CookieKey必填")]
        public string CookieKey { get; set; } = "COOKIE_LOGIN_USER";

        /// <summary>
        /// 成功Url标记
        /// </summary>
        [Required(ErrorMessage = "SuccessFlag必填")]
        public string SuccessFlag { get; set; } = "cloud.189.cn/web/main";

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "UserName必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "Pwd必填")]
        public string Pwd { get; set; }

        /// <summary>
        /// 代理Host
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// 代理端口
        /// </summary>
        public string ProxyPort { get; set; }

        /// <summary>
        /// 代理用户名
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// 代理密码
        /// </summary>
        public string ProxyPwd { get; set; }

        [EnumDataType(typeof(PageLoadType))]
        public PageLoadType PageLoadType { get; set; } = PageLoadType.Normal;
    }

    public enum PageLoadType
    {
        /// <summary>Indicates the behavior is not set.</summary>
        Default,
        /// <summary>
        /// 这是默认策略。在此模式下，WebDriver将等待整个页面加载完成。这包括所有关联的资源，如样式表和图片。
        /// </summary>
        Normal,
        /// <summary>
        /// 在此模式下，WebDriver将在DOM就绪后立即返回。这意味着它不会等待所有关联的资源加载完成。如果你的页面有大量的图片或其他需要时间加载的资源，这种策略可以帮助提高测试的速度。
        /// </summary>
        Eager,
        /// <summary>在此模式下，WebDriver将在HTTP GET操作完成后立即返回。这意味着它不会等待页面加载。这是最快的策略，但也可能导致你尝试访问的元素还没有被加载出来。</summary>
        None,
    }
}
