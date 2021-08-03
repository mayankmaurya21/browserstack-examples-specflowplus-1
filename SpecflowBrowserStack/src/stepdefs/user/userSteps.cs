using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;
using System;

namespace SpecflowBrowserStack.Steps
{
	[Binding]
	public class userSteps
	{
		private readonly WebDriver _driver;
		private static bool result;
		public userSteps(WebDriver driver)
		{
			_driver = driver;
		}

		[Then(@"I should see no image loaded")]
		public void ThenIShouldSeeNoImageLoaded()
		{
			String src = _driver.Current.FindElement(By.XPath("//img[@alt='iPhone 12']")).GetAttribute("src");
			result=FluentAssertions.CustomAssertionAttribute.Equals("img", src);
            _driver.markTestPassFailBrowserStack(result, _driver.Current);
            /* Environment.SetEnvironmentVariable("TEST_INFRA", "");
             string infra = Environment.GetEnvironmentVariable("TEST_INFRA");
             if (infra == "" || infra == null)
             {
                 if (result)
                 {
                     ((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"No Image Displayed\"}}");
                 }
                 else
                 {
                     ((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Image Displayed\"}}");
                 }
             }*/
        }
    }
}
