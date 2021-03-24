using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;
using System;

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
			_driver.Current.Navigate().GoToUrl("https://bstackdemo.com/");
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
			string infra = Environment.GetEnvironmentVariable("TEST_INFRA");
			if (infra == "" || infra == null)
			{
				if (result)
				{
					((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Tests function Assertion Passed\"}}");
				}
				else
				{
					((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Tests function Assertion Failed\"}}");
				}
			}
		}
	}
}
