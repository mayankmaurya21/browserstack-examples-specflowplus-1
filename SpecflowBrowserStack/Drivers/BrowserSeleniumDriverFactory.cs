using System;
using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
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
        private static ExtentReports extent;
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentHtmlReporter htmlReporter;
        private static double epoch;

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
            else if (browserId > 2)
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
            else { return null; }
            // return null;
        }

        public Local GetLocal(int browserIndex)
        {
            if (browserIndex < 2)
            {
                var specificCap = _configurationDriver.Local.ToList<IConfigurationSection>()[0];

                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                    if (tuple.Key.ToString() == "browserstack.local")
                    {
                        Local _local = new Local();
                        List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("key", _configurationDriver.AccessKey)
                    };
                        _local.start(bsLocalArgs);
                        return _local;
                    }
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
                return new FirefoxDriver(@"..\..\browserstack-examples-specflow-1\SpecflowBrowserStack\Drivers\OnPremDriver");
            else if (browser == "chrome")
                return new ChromeDriver(@"..\..\browserstack-examples-specflowplus-1\SpecflowBrowserStack\Drivers\OnPremDriver\");
            else if (browser == "internet explorer")
                return new InternetExplorerDriver(@"..\..\browserstack-examples-specflow-1\SpecflowBrowserStack\Drivers\OnPremDriver");
            else
                return null;
        }

        [BeforeTestRun]
        public static void initializereport()
        {
            htmlReporter = new ExtentHtmlReporter(@"..\..\browserstack-examples-specflow\SpecflowBrowserStack\Extendreport.html");
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        [AfterTestRun]
        public static void teardownReport()
        {
            extent.Flush();
        }
        [BeforeFeature]
        public void BeforeFeature()
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void Initialize(ScenarioContext scenarioContext)
        {
            //Create dynamic scenario name
            scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }
    }
}
