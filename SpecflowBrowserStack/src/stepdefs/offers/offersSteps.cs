using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using DocumentFormat.OpenXml.Drawing;

namespace SpecflowBrowserStack.Steps
{
	[Binding]
	public class offersSteps
	{
		private readonly MobileDriver _driver;
		private static bool result;
		
		public offersSteps(MobileDriver driver)
		{
			_driver = driver;
		}
		[Given(@"I navigate to website on mobile\.")]
		public void GivenINavigateToWebsiteOnMobile_()
		{
			_driver.Current.Value.Navigate().GoToUrl("https://bstackdemo.com/");
			((IJavaScriptExecutor)_driver).ExecuteScript("window.navigator.geolocation.getCurrentPosition = function(cb){cb({ coords: {accuracy: 20,altitude: null,altitudeAccuracy: null,heading: null,latitude: 19.043192,longitude: 72.86305240000002,speed: null}}); }");
		}

		[Then(@"I click on Sign-In link")]
		public void ThenIClickOnSign_InLink()
		{
			_driver.Current.Value.FindElement(By.Id("signin")).Click();
		}

		[When(@"I type ""(.*)"" in username field")]
		public void WhenITypeInUsernameField(string username)
		{
			_driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("react-select-2-input")));
			_driver.Current.Value.FindElement(By.Id("react-select-2-input")).SendKeys(username + "\n");
		}

		[When(@"I type ""(.*)"" in password field")]
		public void WhenITypeInPasswordField(string password)
		{
			_driver.Current.Value.FindElement(By.Id("react-select-3-input")).SendKeys(password + "\n");
		}

		[Then(@"I press Log-In Button")]
		public void ThenIPressLog_InButton()
		{
			_driver.Current.Value.FindElement(By.ClassName("login_password")).Click();
			_driver.Current.Value.FindElement(By.Id("login-btn")).Click();
		}

		[Then(@"I click on Offers link")]
		public void ThenIClickOnOffersLink()
		{
			_driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("offers")));
			_driver.Current.Value.FindElementById("offers").Click();
		}

		[Then(@"I should see Offer elements")]
		public void ThenIShouldSeeOfferElements()
		{
			_driver.Current.Value.Context="NATIVE_APP";
			_driver.Current.Value.FindElementByXPath(".//android.widget.Button[@text='Allow']").Click();
			_driver.Current.Value.Navigate().Refresh();
			String text = _driver.Current.Value.FindElement(By.XPath("//div[@class='p-6 text-2xl tracking-wide text-center text-red-50']")).Text;
			result = FluentAssertions.CustomAssertionAttribute.Equals("We've promotional offers in your location.", text);
		}
	}
}
