using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;

namespace SpecflowBrowserStack.Steps
{
    [Binding]
    public class loginSteps
    {
		private readonly WebDriver _driver;
		private static bool result;

		public loginSteps(WebDriver driver)
		{
			_driver = driver;
		}

		[Given(@"I navigate to website")]
		public void GivenINavigatedTowebsite()
		{
            _driver.Current.Navigate().GoToUrl("https://bstackdemo.com/");
        }
		
		[Then(@"I click on Sign In link")]
		public void ThenIClickOnSignInLink()
		{
			_driver.Current.FindElement(By.Id("signin")).Click();
		}

		[When(@"I type '(.*)' in username")]
		public void ITypeUsername(string username)
		{
			_driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("react-select-2-input")));
			_driver.Current.FindElement(By.Id("react-select-2-input")).SendKeys(username);
			_driver.Current.FindElement(By.Id("react-select-2-input")).SendKeys(Keys.Enter);
		}
		[When(@"I type '(.*)' in password")]
		public void ITypePassword(string password)
		{
			_driver.Current.FindElement(By.Id("react-select-3-input")).SendKeys(password);
			_driver.Current.FindElement(By.Id("react-select-3-input")).SendKeys(Keys.Enter);
		}

		[Then(@"I press Log In Button")]
		public void IPressLogInButton()
		{
			_driver.Current.FindElement(By.Id("login-btn")).Click();
		}
		[Then(@"I should see user '(.*)' logged in")]
		public void IshouldSeeUsername(string username)
		{
			if (username == "locked_user")
			{
				//System.Threading.Thread.Sleep(4000);
				_driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//h3[@class='api-error']")));
				string errorMsg = _driver.Current.FindElement(By.XPath("//h3[@class='api-error']")).Text;
				result = FluentAssertions.CustomAssertionAttribute.Equals(errorMsg, "Your account has been locked.");
			}
			else
			{
				_driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//span[@class='username']")));
				string displayedUsername = _driver.Current.FindElement(By.XPath("//span[@class='username']")).Text;
				result = FluentAssertions.CustomAssertionAttribute.Equals(username, displayedUsername);
			}
			_driver.markTestPassFailBrowserStack(result, _driver.Current);
		}
	}
}
