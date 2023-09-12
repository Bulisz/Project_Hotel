using Hotel.Backend.WebAPI.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Data;
using System.Diagnostics;

namespace HotelBrowserTests
{
    [TestClass]
    public class LoginAndReservationTests
    {
        private const string BaseAddress = "http://localhost:4200";

        private readonly By _registerButtonLocator = By.Id("registerButton");
        private readonly By _userNameInputLocator = By.Id("userName");
        private readonly By _firstNameInputLocator = By.Id("kersztNev");
        private readonly By _lastNameInputLocator = By.Id("csaladNev");
        private readonly By _emailInputLocator = By.Id("emailInpt");
        private readonly By _passwordInputLocator = By.Id("passwordInput");
        private readonly By _passwordAgainInputLocator = By.Id("passwordAgain");
        private readonly By _sendLoginDetailsButtonLocator = By.Id("sendButton");
        //private readonly By _homeLoanOptionsLocator = By.XPath("//select[@id='homeLoanOptions']");


        private IWebDriver _driver = null!;

        private  HotelDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            _driver = new ChromeDriver();
            var builder = new DbContextOptionsBuilder<HotelDbContext>();
            builder.UseSqlite("Data Source = :memory:"); 
            _context = new HotelDbContext(builder.Options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void ClearDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void LoginTest()
        {
            // Open the webpage
            _driver.Navigate().GoToUrl(BaseAddress);
            Wait(_driver);

            // Click on register button
            var registerButton = _driver.FindElement(_registerButtonLocator);
            registerButton.Click();

            // Enter the firstName
            var firstNameInput = _driver.FindElement(_firstNameInputLocator);
            firstNameInput.SendKeys("black");
            Wait(_driver);

            // Enter the lastName
            var lastNameInput = _driver.FindElement(_lastNameInputLocator);
            lastNameInput.SendKeys("heeler");
            Wait(_driver);

            // Enter the userName
            var userNameInput = _driver.FindElement(_userNameInputLocator);
            userNameInput.SendKeys("blackheeler");
            Wait(_driver);

            // Enter the email
            var emailInput = _driver.FindElement(_emailInputLocator);
            emailInput.SendKeys("blackheeler@gmai.com");
            Wait(_driver);

            // Enter the password
            var passwordInput = _driver.FindElement(_passwordInputLocator);
            passwordInput.SendKeys("123456");
            Wait(_driver);

            // Enter the passwordAgain
            var passwordAgain = _driver.FindElement(_passwordAgainInputLocator);
            passwordAgain.SendKeys("123456");
            Wait(_driver);

            // Click on send button
            var sendButton = _driver.FindElement(_sendLoginDetailsButtonLocator);
            sendButton.Click();

            Wait2(_driver);
            // patient.component.html

        }



    [Conditional("DEBUG")] //'Release'
        private static void Wait(IWebDriver driver)
        {
            new Actions(driver).Pause(TimeSpan.FromSeconds(2)).Perform();
        }
        private static void Wait2(IWebDriver driver)
        {
            new Actions(driver).Pause(TimeSpan.FromSeconds(10)).Perform();
        }
    }
}