using CloudinaryDotNet.Actions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog.Layouts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Data;
using System.Diagnostics;
using static OpenQA.Selenium.Interactions.WheelInputDevice;

namespace HotelBrowserTests
{
    [TestClass]
    public class SearchAndRegistrationTests
    {
        private const string BaseAddress = "http://localhost:4200";
        //private const string BaseAddress = "https://doghotel-001-site1.dtempurl.com";

        private readonly By _registerButtonLocator = By.Id("registerButton");
        private readonly By _userNameInputLocator = By.Id("userName");
        private readonly By _firstNameInputLocator = By.Id("kersztNev");
        private readonly By _lastNameInputLocator = By.Id("csaladNev");
        private readonly By _emailInputLocator = By.Id("emailInpt");
        private readonly By _passwordInputLocator = By.Id("passwordInput");
        private readonly By _passwordAgainInputLocator = By.Id("passwordAgain");
        private readonly By _sendLoginDetailsButtonLocator = By.Id("sendButton");
        private readonly By _aboutusNavbarLocator = By.Id("about-us");
        private readonly By _blogNavbarLocator = By.Id("blog");
        private readonly By _eventlistNavbarLocator = By.Id("event-list");
        private readonly By _roomlistNavbarLocator = By.Id("room-list");
        private readonly By _numberOfGuestsInputLocator = By.Id("numberOfBeds");
        private readonly By _numberOfDogsInputLocator = By.Id("maxNumberOfDogs");
        private readonly By _bookingFromInputLocator = By.Id("bookingFrom");
        private readonly By _bookingToInputLocator = By.Id("bookingTo");
        private readonly By _submitSearchButtonLocator = By.Id("submitSearch");
        //private readonly By _XPathLocator = By.XPath("//select[@id='SomethingId']");


        private IWebDriver _driver = null!;

        private HotelDbContext _context;

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
        public void SearchAndRegistrationTest()
        {
            // Open the webpage
            _driver.Navigate().GoToUrl(BaseAddress);
            Wait(_driver);

            //NAVBAR

            // Click on about-us on navbar
            var aboutusNavbar = _driver.FindElement(_aboutusNavbarLocator);
            aboutusNavbar.Click();
            Wait(_driver);

            // Click on blog on navbar
            var blogNavbar = _driver.FindElement(_blogNavbarLocator);
            blogNavbar.Click();
            Wait(_driver);

            // Click on event-list on navbar
            var eventlistNavbar = _driver.FindElement(_eventlistNavbarLocator);
            eventlistNavbar.Click();
            Wait(_driver);

            // Click on room-list on navbar
            var roomlistNavbar = _driver.FindElement(_roomlistNavbarLocator);
            roomlistNavbar.Click();
            Wait(_driver);

            //gorgetes.....
            var scrollOrigin = new WheelInputDevice.ScrollOrigin
            {
                Viewport = true,
                XOffset = 10,
                YOffset = 10
            };
            ScrollDown(_driver, scrollOrigin);
            ScrollDown(_driver, scrollOrigin);
            ScrollDown(_driver, scrollOrigin);
            ScrollDown(_driver, scrollOrigin);
            ScrollDown(_driver, scrollOrigin);
            ScrollUp(_driver, scrollOrigin);

            Wait2(_driver);

            //SEARCHROOM
            // Enter the numberOfGuests
            var numberOfGuestsInput = _driver.FindElement(_numberOfGuestsInputLocator);
            numberOfGuestsInput.SendKeys("2");
            Wait(_driver);

            // Enter the numberOfDogs
            var numberOfDogsInput = _driver.FindElement(_numberOfDogsInputLocator);
            numberOfDogsInput.SendKeys("4");
            Wait(_driver);

            // Enter the bookingFromInputDate
            var bookingFromInput = _driver.FindElement(_bookingFromInputLocator);
            bookingFromInput.SendKeys("002023/10/2");
            Wait(_driver);

            // Enter the bookingToInputDate
            var bookingToInput = _driver.FindElement(_bookingToInputLocator);
            bookingToInput.SendKeys("002023/10/12");
            Wait(_driver);

            // Click on search button
            var submitSearchButton = _driver.FindElement(_submitSearchButtonLocator);
            submitSearchButton.Click();
            Wait(_driver);

            ScrollDown(_driver, scrollOrigin);
            ScrollUp(_driver, scrollOrigin);

            Wait2(_driver);

            //REGISTER

            // Click on register button
            var registerButton = _driver.FindElement(_registerButtonLocator);
            registerButton.Click();
            Wait(_driver);

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
            new Actions(driver).Pause(TimeSpan.FromSeconds(4)).Perform();
        }
        private static void ScrollDown(IWebDriver driver1, WheelInputDevice.ScrollOrigin scrollOrigin)
        {
            new Actions(driver1).Pause(TimeSpan.FromSeconds(1)).ScrollFromOrigin(scrollOrigin, 0, 800).Perform();
        }
        private static void ScrollUp(IWebDriver driver1, WheelInputDevice.ScrollOrigin scrollOrigin)
        {
            new Actions(driver1).Pause(TimeSpan.FromSeconds(1)).ScrollFromOrigin(scrollOrigin, 0, -4200).Perform();
        }
    }
}