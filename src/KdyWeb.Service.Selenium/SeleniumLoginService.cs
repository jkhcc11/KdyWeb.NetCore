using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Dto.Selenium;
using KdyWeb.IService.Selenium;
using KdyWeb.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KdyWeb.Service.Selenium
{
    /// <summary>
    /// Selenium模拟登录 服务实现
    /// </summary>
    public class SeleniumLoginService : BaseWebDriverBase, ISeleniumLoginService
    {
        private readonly ILogger<SeleniumLoginService> _logger;
        private readonly IConfiguration _configuration;

        public SeleniumLoginService(ILogger<SeleniumLoginService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// 天翼云登录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<string>> LoginWithTyPersonAsync(LoginBySeleniumInput input)
        {
            IWebDriver? webDriver = null;
            try
            {
                var cookieKeys = input.CookieKey.Split(',');

                var options = BuildChromeOptions(Enum.Parse<PageLoadStrategy>(input.PageLoadType.ToString()),
                    isIncognito: true);
                webDriver = BuildRemoteWebDriver(options,
                    _configuration.GetValue<string>(WebDriverUrl));
                webDriver.Navigate().GoToUrl(input.LoginUrl);

                //延迟2秒 为了后面的xpath定位
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(2000);

                #region 定位Iframe
                //默认第一层 有可能嵌套了iframe
                IWebDriver iWebDriver = webDriver;
                int i = 3;

                while (i-- > 0)
                {
                    var isFrame = iWebDriver.FindElements(By.XPath("//iframe"));
                    if (isFrame is { Count: > 0 })
                    {
                        for (var j = 0; j < isFrame.Count; j++)
                        {
                            #region 多个iframe只要有值的
                            var item = isFrame[j];
                            var src = item.GetAttribute("src");
                            if (string.IsNullOrEmpty(src))
                            {
                                //说明是假的iframe 跳过
                                continue;
                            }
                            //说明存在iframe 切换
                            iWebDriver = iWebDriver.SwitchTo().Frame(j);
                            break;
                            #endregion
                        }
                        continue;
                    }

                    break;
                }

                if (i == 0)
                {
                    //说明向下三层没有找到
                    return KdyResult.Error<string>(KdyResultCode.Error, "未找到Iframe");
                }
                #endregion

                //切换普通登录
                var tabElement = WaitForElement(iWebDriver, By.XPath("//*[@id='tab-pw']"));
                tabElement?.Click();

                //获取用户名、密码框、登录按钮
                var userElement = WaitForElement(iWebDriver, By.XPath("//*[@id='userName']"));
                var pwdElement = WaitForElement(iWebDriver, By.XPath("//*[@id='password']"));
                var loginElement = WaitForElement(iWebDriver, By.XPath("//*[@id='j-login']"));
                var agreementElement = WaitForElement(iWebDriver, By.XPath("//*[@id='j-agreement-box']"));

                userElement?.SendKeys(input.UserName);
                await Task.Delay(1500);
                pwdElement?.SendKeys(input.Pwd);
                agreementElement?.Click();
                await Task.Delay(1500);
                loginElement?.Click();

                //延迟
                await Task.Delay(1900);

                var currentUrl = webDriver.Url;
                if (currentUrl.Contains(input.SuccessFlag))
                {
                    var resultData = "";
                    var cookies = webDriver.Manage().Cookies.AllCookies;
                    foreach (var item in cookies)
                    {
                        if (cookieKeys.Count(a => a.ToLower() == item.Name.ToLower()) > 0)
                        {
                            resultData += $"{item.Name}={item.Value};";
                        }
                    }

                    _logger.LogInformation("LoginUrl:{loginUrl}\r\nInput：{input}\r\nCookie返回:{result}", webDriver.Url,
                        input.ToJsonStr(), resultData);
                    return KdyResult.Success(resultData, "操作成功");
                }

                return KdyResult.Error<string>(KdyResultCode.Error, $"未知跳转Url,{currentUrl}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登录失败异常.Input:{input}", input.ToJsonStr());
                return KdyResult.Error<string>(KdyResultCode.Error, $"登录失败，异常：{ex.Message}");
            }
            finally
            {
                webDriver?.Close();
                webDriver?.Quit();
            }
        }

        /// <summary>
        /// 天翼云H5登录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<string>> LoginWithTyH5Async(LoginBySeleniumInput input)
        {
            var userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1";

            IWebDriver? webDriver = null;
            try
            {
                
                var options = BuildChromeOptions(Enum.Parse<PageLoadStrategy>(input.PageLoadType.ToString()),
                    isIncognito: true);
                SetMobileModel(options, userAgent);
                webDriver = BuildRemoteWebDriver(options,
                    _configuration.GetValue<string>(WebDriverUrl));
                webDriver.Navigate().GoToUrl(input.LoginUrl);

                //清空本地storage //开启无痕后 不用管理cookie等
                //await Task.Delay(1500);
                var javaScriptExecutor = (IJavaScriptExecutor)webDriver;
                //javaScriptExecutor.ExecuteScript("localStorage.clear();");

                var btnH5Element = WaitForElement(webDriver, By.XPath("//*[@class='smart-network-btn']"));
                btnH5Element?.Click();

                //这里有段检测 需要延迟久一点
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30)); // 设置最大等待时间为30秒

                string? oldUrl;
                var newUrl = webDriver.Url;
                // 等待 URL 停止改变
                await wait.Until(async driver =>
                {
                    oldUrl = newUrl;
                    await Task.Delay(5000);
                    newUrl = driver.Url;
                    return oldUrl == newUrl;
                });

                var btnOtherElement = WaitForElement(webDriver, By.XPath("//*[@id='j-modal']/div/a[2]"));
                if (btnOtherElement != null)
                {
                    //未找到 可能直接进来了
                    #region 登录逻辑
                    btnOtherElement.Click();
                    //延迟后点击切换账号密码登录
                    var btnPwdElement = WaitForElement(webDriver, By.XPath("//*[@id='j-account-login-link']"));
                    btnPwdElement?.Click();

                    //开始输入
                    //获取用户名、密码框、登录按钮
                    var userElement = WaitForElement(webDriver, By.XPath("//*[@id='j-userName']"));
                    var pwdElement = WaitForElement(webDriver, By.XPath("//*[@id='j-password']"));
                    var loginElement = WaitForElement(webDriver, By.XPath("//*[@id='j-login-btn']"));
                    var agreementElement = WaitForElement(webDriver, By.XPath("//*[@id='j-agreement-checkbox']"));

                    userElement?.SendKeys(input.UserName);
                    await Task.Delay(1500);
                    pwdElement?.SendKeys(input.Pwd);
                    agreementElement?.Click();
                    await Task.Delay(1500);
                    loginElement?.Click();
                    #endregion
                }

                //https://h5.cloud.189.cn/main.html#/family/list
                //这里必须硬等待 否则获取不到localStorage
                await Task.Delay(2000);
                var currentUrl = webDriver.Url;
                if (currentUrl.Contains(input.SuccessFlag))
                {
                    var tempToken = javaScriptExecutor.ExecuteScript("return localStorage.getItem('accessToken');") + "";
                    tempToken = tempToken.Trim('"');
                    _logger.LogInformation("LoginUrl:{loginUrl}\r\nInput：{input}\r\nToken返回:{result}",
                        webDriver.Url,
                        input.ToJsonStr(),
                        tempToken);
                    return KdyResult.Success(tempToken, "操作成功");
                }

                return KdyResult.Error<string>(KdyResultCode.Error, $"h5未知跳转Url,{currentUrl}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "h5登录失败异常.Input:{input}", input.ToJsonStr());
                return KdyResult.Error<string>(KdyResultCode.Error, $"h5登录失败，异常：{ex.Message}");
            }
            finally
            {
                //每次请求完 杀死进程
                webDriver?.Close();
                webDriver?.Quit();
            }
        }

    }
}
