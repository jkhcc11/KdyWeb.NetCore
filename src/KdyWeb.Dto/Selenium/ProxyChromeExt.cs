namespace KdyWeb.Dto.Selenium;

public class ProxyChromeExt
{
    public const string ManifestJson = $@"
    {{
        ""version"": ""1.0.0"",
        ""manifest_version"": 2,
        ""name"": ""Chrome Proxy"",
        ""permissions"": [
            ""proxy"",
            ""tabs"",
            ""unlimitedStorage"",
            ""storage"",
            ""<all_urls>"",
            ""webRequest"",
            ""webRequestBlocking""
        ],
        ""background"": {{
            ""scripts"": [""background.js""]
        }},
        ""minimum_chrome_version"":""22.0.0""
    }}";

    //port 必须是数字格式  不能是string
    public const string BackgroundJs = $@"
    var config = {{
            mode: ""fixed_servers"",
            rules: {{
            singleProxy: {{
                scheme: ""http"",
                host: ""{{proxy_host}}"",
                port: {{proxy_port}}
            }},
            bypassList: [""localhost""]
            }}
        }};

    chrome.proxy.settings.set({{value: config, scope: ""regular""}}, function() {{}});

    function callbackFn(details) {{
        return {{
            authCredentials: {{
                username:""{{proxy_user}}"",
                password: ""{{proxy_pwd}}""
            }}
        }};
    }}

    chrome.webRequest.onAuthRequired.addListener(
                callbackFn,
                {{urls: [""<all_urls>""]}},
                ['blocking']
    );";
}