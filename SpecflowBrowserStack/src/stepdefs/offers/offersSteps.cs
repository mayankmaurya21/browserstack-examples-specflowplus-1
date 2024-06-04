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
		private readonly WebDriver _driver;
		private static bool result;
		
		public offersSteps(WebDriver driver)
		{
			_driver = driver;
		}

		[Given(@"I navigate to website with mumbai geo-location")]
		public void GivenINavigateToWebsiteWithMumbaiGeo_Location()
		{
			_driver.Current.Navigate().GoToUrl("http://localhost:3000/");
			((IJavaScriptExecutor)_driver.Current).ExecuteScript("window.navigator.geolocation.getCurrentPosition = function(cb){cb({ coords: {accuracy: 20,altitude: null,altitudeAccuracy: null,heading: null,latitude: 19.043192,longitude: 75.86305240000002,speed: null}}); }");
		}

		[Then(@"I click on Offers link")]
		public void ThenIClickOnOffersLink()
		{
			_driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("offers")));
			_driver.Current.FindElement(By.Id("offers")).Click();
		}

		[Then(@"I should see Offer elements")]
		public void ThenIShouldSeeOfferElements()
		{
			String text = _driver.Current.FindElement(By.XPath("//div[@class='p-6 text-2xl tracking-wide text-center text-red-50']")).Text;
			result = FluentAssertions.CustomAssertionAttribute.Equals("We've promotional offers in your location.", text);
			_driver.markTestPassFailBrowserStack(result, _driver.Current);
		}
	}
}
