using System;
using System.Collections.Generic;
using System.Linq;
using BrowserStack;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;
using TechTalk.SpecRun;

namespace SpecflowBrowserStack.Drivers
{
    public class BrowserSeleniumDriverFactory
    {
        private readonly ConfigurationDriver _configurationDriver;
        private readonly TestRunContext _testRunContext;
        string remoteUrl = "";
        private FeatureContext _featureContext;
        private ScenarioContext _scenarioContext;

        public BrowserSeleniumDriverFactory(ConfigurationDriver configurationDriver, TestRunContext testRunContext, FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _configurationDriver = configurationDriver;
            _testRunContext = testRunContext;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        public IWebDriver GetForBrowser(int browserId)
        {
           
            // sets remote URL
            string username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username == null || username == "")
            {
                username = _configurationDriver.Username;
            }
            string access_key = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (access_key == null || access_key == "")
            {
                access_key = _configurationDriver.AccessKey;
            }
            string infra = Environment.GetEnvironmentVariable("TEST_INFRA");

            if (infra == "DOCKER")
            {
                remoteUrl = _configurationDriver.SeleniumBaseUrl + "/wd/hub/";
            }
            else
            {
                remoteUrl = "https://" + username + ":" + access_key + "@" + _configurationDriver.SeleniumBaseUrl + "/wd/hub/";
            }

            DesiredCapabilities caps = new DesiredCapabilities();
            // Set common capabilities like "browserstack.local", project, name, session
            foreach (var tuple in _configurationDriver.CommonCapabilities)
            {
                caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
            }
            if (browserId == 0)
            {
                var specificCap = _configurationDriver.Single.ToList<IConfigurationSection>()[browserId];
                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                    if (tuple.Value.ToString() == "chrome" && _featureContext.FeatureInfo.Title == "Offers Feature")
                    {
                        Dictionary<string, object> profile = new Dictionary<string, object>();

                        // 0 - Default, 1 - Allow, 2 - Block
                        profile.Add("profile.default_content_setting_values.geolocation", 1);

                        // INIT CHROME OPTIONS
                        Dictionary<string, object> chromeOptions = new Dictionary<string, object>();

                        // SET CHROME OPTIONS
                        chromeOptions.Add("prefs", profile);
                        caps.SetCapability("chromeOptions", chromeOptions);
                    }

                    if (tuple.Key.ToString() == "name")
                    {
                        caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + _scenarioContext.ScenarioInfo.Title);
                    }
                    if (infra == "ON_PREM" && tuple.Key.ToString() == "browser")
                    {
                        return OnPremDrivers(tuple.Value.ToString());
                    }
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
            else if (browserId == 1)
            {
                var specificCap = _configurationDriver.Local.ToList<IConfigurationSection>()[browserId - 1];
                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                    if (tuple.Key.ToString() == "name")
                    {
                        caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + _scenarioContext.ScenarioInfo.Title);
                    }
                    if (infra == "ON_PREM" && tuple.Key.ToString() == "browser")
                    {
                        return OnPremDrivers(tuple.Value.ToString());
                    }
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
            else if (browserId == 2)
            {
                // Set session specific capability
                var specificCaps = _configurationDriver.Mobile.ToList<IConfigurationSection>()[browserId - 2];
                foreach (var tuple in specificCaps.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                    if (tuple.Key.ToString() == "name")
                    {
                        caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + _scenarioContext.ScenarioInfo.Title);
                    }
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
            else if (browserId > 2 && browserId < 6)
            {
                // Set session specific capability
                var specificCaps = _configurationDriver.Parallel.ToList<IConfigurationSection>()[browserId - 3];
                foreach (var tuple in specificCaps.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                    if (tuple.Key.ToString() == "name")
                    {
                        caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + _scenarioContext.ScenarioInfo.Title);
                    }
                    if (infra == "ON_PREM" && tuple.Key.ToString() == "browser")
                    {
                        return OnPremDrivers(tuple.Value.ToString());
                    }
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
            else if (browserId > 5 && browserId < 8)
            {
                // Set session specific capability
                var specificCaps = _configurationDriver.Local_Parallel.ToList<IConfigurationSection>()[browserId - 6];
                foreach (var tuple in specificCaps.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                    if (tuple.Key.ToString() == "name")
                    {
                        caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + _scenarioContext.ScenarioInfo.Title);
                    }
                    if (infra == "ON_PREM" && tuple.Key.ToString() == "browser")
                    {
                        return OnPremDrivers(tuple.Value.ToString());
                    }
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
            else { return null; }
            // return null;
        }

        public Local GetLocal(int browserIndex)
        {
            string access_key = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            string infra = Environment.GetEnvironmentVariable("TEST_INFRA");
            if (access_key == null || access_key == "")
            {
                access_key = _configurationDriver.AccessKey;
            }
            if ((browserIndex == 1 || (browserIndex > 5 && browserIndex < 8)) && (infra == null || infra == ""))
            {
                var specificCap = _configurationDriver.Local.ToList<IConfigurationSection>()[0];

                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                int i = 0;
                if (tuple.Key.ToString() == "browserstack.local")
                    {
                    
                        Local _local = new Local();
                        List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("key", access_key),
                        new KeyValuePair<string, string>("local-identifier", "identifier-unique-name"+i.ToString())
                        
                    };
                        _local.start(bsLocalArgs);
                        return _local;
                    }
                i += 1;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public IWebDriver OnPremDrivers(String browser)
        {
            if (browser == "firefox")
                return new FirefoxDriver(@"..\..\SpecflowBrowserStack\Drivers\OnPremDriver");
            else if (browser == "chrome")
                return new ChromeDriver(@"..\..\SpecflowBrowserStack\Drivers\OnPremDriver\");
            else if (browser == "internet explorer")
            {
                var options = new InternetExplorerOptions();
                options.IgnoreZoomLevel = true;
                return new InternetExplorerDriver(@"..\..\SpecflowBrowserStack\Drivers\OnPremDriver", options);
            }
            else
                return null;
        }
    }
}
