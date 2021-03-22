using System;
using System.Diagnostics;
using System.Linq;
using BrowserStack;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace SpecflowBrowserStack.Drivers
{
    public class MobileDriver : IDisposable
	{
		private readonly BrowserSeleniumDriverFactory _browserSeleniumDriverFactory;
		private readonly Lazy<AndroidDriver<AndroidElement>> _currentWebDriverLazy;
		private readonly Lazy<WebDriverWait> _waitLazy;
		private Local _currentLocal;
		private readonly TimeSpan _waitDuration = TimeSpan.FromSeconds(10);
		private bool _isDisposed;
		
		public MobileDriver(BrowserSeleniumDriverFactory browserSeleniumDriverFactory)
		{
			_browserSeleniumDriverFactory = browserSeleniumDriverFactory;
			_currentWebDriverLazy = new Lazy<AndroidDriver<AndroidElement>>(GetMobileWebDriver);
			
		}
		public Lazy<AndroidDriver<AndroidElement>> Current => _currentWebDriverLazy;
		public WebDriverWait Wait => _waitLazy.Value;

        private WebDriverWait GetWebDriverWait()
        {
            return new WebDriverWait((IWebDriver)Current, _waitDuration);
        }
        private AndroidDriver<AndroidElement> GetMobileWebDriver()
		{

			string browserIndex = Environment.GetEnvironmentVariable("Test_Browser_Index");
			if (browserIndex == null)
			{
				browserIndex = "0";
			}
			int testBrowserId = Convert.ToInt32(browserIndex);

            if (_currentLocal == null)
            {
                _currentLocal = GetBrowserStackLocal();
            }
            return _browserSeleniumDriverFactory.GetForMobileBrowser(testBrowserId);
		}

		private Local GetBrowserStackLocal()
		{
			string browserIndex = Environment.GetEnvironmentVariable("Test_Browser_Index");
            if (browserIndex == null)
            {
                browserIndex = "0";
            }
            int testBrowserId = Convert.ToInt32(browserIndex);
			return _browserSeleniumDriverFactory.GetLocal(testBrowserId);
		}
		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			if (_currentWebDriverLazy.IsValueCreated)
			{
				Current.Value.Quit();
                if (_currentLocal != null && Process.GetProcessesByName("BrowserStackLocal").Any())
                {
                    _currentLocal.stop();
                }
            }
			_isDisposed = true;
		}
	}
}