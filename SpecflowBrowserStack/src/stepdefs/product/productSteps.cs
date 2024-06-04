using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;
using System;
using Xunit;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports.Gherkin.Model;

namespace SpecflowBrowserStack.Steps
{
    [Binding]
	public class productSteps
	{
		private readonly WebDriver _driver;
        private static bool result=true;
      
        public productSteps(WebDriver driver)
		{
			_driver = driver;
        }
        [Given(@"I navigate to website locally")]
        public void GivenINavigateToWebsiteLocally()
        {
            _driver.Current.Navigate().GoToUrl("http://localhost:3000/");
        }


        [Given(@"I press the Apple Vendor Filter")]
        public void GivenIPressTheAppleVendorFilter()
        {
            _driver.Current.FindElement(By.XPath("//span[@class='checkmark' and text()='Apple']")).Click();  
        }

        [Then(@"I should see (.*) items in the list")]
        public void ThenIShouldSeeItemsInTheList(int noOfproducts)
        { 
            string numberOfProducts = _driver.Current.FindElement(By.XPath("//small[@class='products-found']")).Text;
           bool assert= FluentAssertions.CustomAssertionAttribute.Equals(noOfproducts+" Product(s) found.", numberOfProducts);
           _driver.markTestPassFailBrowserStack(assert, _driver.Current);
        }

        [Given(@"I order by lowest to highest")]
        public void GivenIOrderByLowestToHighest()
        {
            IWebElement dropDown = _driver.Current.FindElement(By.XPath("//select"));
            SelectElement select = new SelectElement(dropDown);
            select.SelectByText("Lowest to highest"); 
        }

        [Then(@"I should see prices in ascending order")]
        public void ThenIShouldSeePricesInAscendingOrder()
        {
           String fristElementPrice= _driver.Current.FindElement(By.XPath("(//div[@class='val']//b)[1]")).Text;
           String secondElementPrice = _driver.Current.FindElement(By.XPath("(//div[@class='val']//b)[2]")).Text;
           Assert.True(Convert.ToInt32(fristElementPrice) < Convert.ToInt32(secondElementPrice));
            _driver.markTestPassFailBrowserStack(result, _driver.Current);
        }
    }
}
