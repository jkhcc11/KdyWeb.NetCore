using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using KdyWeb.Dto.Selenium;
using System.IO.Compression;
using OpenQA.Selenium.Support.UI;

namespace KdyWeb.Service.Selenium
{
    /// <summary>
    /// Selenium 驱动基类
    /// </summary>
    public abstract class BaseWebDriverBase
    {
        public const string WebDriverUrl = "RemoteWebDriverUrl";

        /// <summary>
        /// 实例化RemoteWebDriver
        /// </summary>
        /// <returns></returns>
        protected virtual IWebDriver BuildRemoteWebDriver(ChromeOptions? options = null, string? webDriverUri = null)
        {
            options ??= BuildChromeOptions();
            //远程WebDriver 地址
            var uri = new Uri(webDriverUri ?? "http://localhost:6789");
            return new RemoteWebDriver(uri, options);
        }

        /// <summary>
        /// 生成Options
        /// </summary>
        /// <param name="loadStrategy">
        ///  Normal：这是默认策略。在此模式下，WebDriver将等待整个页面加载完成。这包括所有关联的资源，如样式表和图片。<br/>
        /// /Eager：在此模式下，WebDriver将在DOM就绪后立即返回。这意味着它不会等待所有关联的资源加载完成。如果你的页面有大量的图片或其他需要时间加载的资源，这种策略可以帮助提高测试的速度。<br/>
        /// None：在此模式下，WebDriver将在HTTP GET操作完成后立即返回。这意味着它不会等待页面加载。这是最快的策略，但也可能导致你尝试访问的元素还没有被加载出来。<br/>
        /// </param>
        /// <param name="isHeadless">是否隐藏浏览器窗口</param>
        /// <param name="isIncognito">是否无痕模式</param>
        /// <param name="isDisableImg">是否禁止加载图片</param>
        /// <param name="userAgent">UserAgent</param>
        /// <returns></returns>
        protected virtual ChromeOptions BuildChromeOptions(PageLoadStrategy loadStrategy = PageLoadStrategy.Normal,
            bool isHeadless = false,
            bool isIncognito = false,
            bool isDisableImg = false,
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36")
        {
            var options = new ChromeOptions();
            //语言
            options.AddArguments("lang=zh_CN.UTF-8");

            options.PageLoadStrategy = loadStrategy;
            options.AddArguments("--disable-gpu"
                // "--disable-dev-shm-usage",  docker会出问题
                //  "--no-zygote" docker会出问题
                );
            if (isIncognito)
            {
                options.AddArguments("--incognito");
            }

            if (isHeadless)
            {
                //隐藏弹窗浏览器
                options.AddArguments("--headless");
            }

            if (isDisableImg)
            {
                //禁用图片
                options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            }

            //电脑版 固定浏览器
            options.AddArgument($"--user-agent={userAgent}");
            return options;
        }

        /// <summary>
        /// 设置手机模拟参数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userAgent">Agent</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="pixelRatio">比例</param>
        /// <returns></returns>
        protected virtual ChromeOptions SetMobileModel(ChromeOptions options, string userAgent,
            int width = 640, int height = 800, double pixelRatio = 1.0)
        {
            var mobileEmulationDeviceSettings = new ChromiumMobileEmulationDeviceSettings
            {
                Width = width,
                Height = height,
                PixelRatio = pixelRatio,
                UserAgent = userAgent
            };
            options.EnableMobileEmulation(mobileEmulationDeviceSettings);

            return options;
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        protected virtual ChromeOptions SetProxy(ChromeOptions options,
            string proxyHost, string proxyPort, string? user, string? pwd)
        {
            // 设置代理
            var proxy = new Proxy
            {
                //如果这里是隧道代理时且用户名和密码方式，是不支持的，除非支持白名单模式才可以
                HttpProxy = $"http://{proxyHost}:{proxyPort}",
                SslProxy = $"http://{proxyHost}:{proxyPort}"
            };
            options.Proxy = proxy;
            if (string.IsNullOrEmpty(user) ||
                string.IsNullOrEmpty(user))
            {
                return options;
            }

            //如果有密码 需要加一个扩展 todo:待调试 这里有问题
            var tempBackJs = ProxyChromeExt.BackgroundJs
                .Replace("{proxy_host}", proxyHost)
                .Replace("{proxy_port}", proxyPort)
                .Replace("{proxy_user}", user)
                .Replace("{proxy_pwd}", pwd);
            var pluginFile = "proxy_auth_plugin.zip";
            using (var archive = new ZipArchive(File.OpenWrite(pluginFile), ZipArchiveMode.Create))
            {
                var manifestEntry = archive.CreateEntry("manifest.json");
                using (var writer = new StreamWriter(manifestEntry.Open()))
                {
                    writer.Write(ProxyChromeExt.ManifestJson);
                }

                var backgroundEntry = archive.CreateEntry("background.js");
                using (var writer = new StreamWriter(backgroundEntry.Open()))
                {
                    writer.Write(tempBackJs);
                }
            }

            options.AddExtension(pluginFile);
            return options;
        }
        
        /// <summary>
        /// 延迟等待获取节点信息
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="selector">By</param>
        /// <param name="waitSecond">等待秒 默认5秒</param>
        /// <returns></returns>
        protected virtual IWebElement? WaitForElement(IWebDriver driver, By selector, int waitSecond = 5)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSecond));
            var el = driver.FindElement(selector);
            wait.Until(d => el.Displayed);
            return el;
        }
    }
}
