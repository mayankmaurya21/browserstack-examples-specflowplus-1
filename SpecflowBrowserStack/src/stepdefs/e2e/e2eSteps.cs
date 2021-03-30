using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;
using System;

namespace SpecflowBrowserStack.Steps
{
    [Binding]
    public class e2eSteps

    {
        private readonly WebDriver _driver;
        private bool result;

        public e2eSteps(WebDriver driver)
        {
            _driver = driver;
        }

        [When(@"I type ""(.*)"" in username")]
        public void WhenITypeInUsername(string username)
        {
            _driver.Current.FindElement(By.Id("react-select-2-input")).SendKeys(username);
            _driver.Current.FindElement(By.Id("react-select-2-input")).SendKeys(Keys.Enter);
        }

        [When(@"I type ""(.*)"" in password")]
        public void WhenITypeInPassword(string password)
        {
            _driver.Current.FindElement(By.Id("react-select-3-input")).SendKeys(password);
            _driver.Current.FindElement(By.Id("react-select-3-input")).SendKeys(Keys.Enter);
        }

        [Then(@"I add two products to cart")]
        public void ThenIAddTwoProductsToCart()
        {
            _driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("(//div[text()='Add to cart'])[1]")));
            _driver.Current.FindElement(By.XPath("(//div[text()='Add to cart'])[1]")).Click();
            _driver.Current.FindElement(By.XPath("(//div[text()='Add to cart'])[1]")).Click();

        }

        [Then(@"I click on Buy Button")]
        public void ThenIClickOnBuyButton()
        {
            _driver.Current.FindElement(By.XPath("//div[text()='Checkout']")).Click();
        }

        [When(@"I type ""(.*)"" in firstNameInput input")]
        public void WhenITypeInFirstNameInputInput(string firstName)
        {
            _driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("firstNameInput")));
            _driver.Current.FindElement(By.Id("firstNameInput")).SendKeys(firstName);
        }

        [When(@"I type ""(.*)"" in lastNameInput input")]
        public void WhenITypeInLastNameInputInput(string lastName)
        {
            _driver.Current.FindElement(By.Id("lastNameInput")).SendKeys(lastName);
        }

        [When(@"I type ""(.*)"" in addressLineInput input")]
        public void WhenITypeInAddressLineInputInput(string address)
        {
            _driver.Current.FindElement(By.Id("addressLine1Input")).SendKeys(address);
        }

        [When(@"I type ""(.*)"" in provinceInput input")]
        public void WhenITypeInProvinceInputInput(string province)
        {
            _driver.Current.FindElement(By.Id("provinceInput")).SendKeys(province);
        }

        [When(@"I type ""(.*)"" in postCodeInput input")]
        public void WhenITypeInPostCodeInputInput(int postalCode)
        {
            _driver.Current.FindElement(By.Id("postCodeInput")).SendKeys(postalCode.ToString());
        }

        [Then(@"I click on Checkout Button")]
        public void ThenIClickOnCheckoutButton()
        {
            _driver.Current.FindElement(By.Id("checkout-shipping-continue")).Click();
            _driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[text()='Continue Shopping »']")));
            _driver.Current.FindElement(By.XPath("//button[text()='Continue Shopping »']")).Click();
        }

        [Then(@"I click on ""(.*)"" link")]
        public void ThenIClickOnLink(string orders)
        {
            _driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("orders")));
            _driver.Current.FindElement(By.Id("orders")).Click();
        }

        [Then(@"I should see elements in list")]
        public void ThenIShouldSeeElementsInList()
        {
            _driver.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("(//span[@class='a-color-secondary label' ])[1]")));
            string orderPlaced = _driver.Current.FindElement(By.XPath("(//span[@class='a-color-secondary label' ])[1]")).Text;
            result = FluentAssertions.CustomAssertionAttribute.Equals("order placed", orderPlaced.ToLower());
            _driver.markTestPassFailBrowserStack(result, _driver.Current);
        }
    }
}
