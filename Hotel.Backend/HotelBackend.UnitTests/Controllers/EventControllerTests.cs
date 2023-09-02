using AutoMapper;
using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Services;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Hotel.Backend.WebAPI.Helpers;
using System.Net;

namespace HotelBackend.UnitTests.Controllers
{
    [TestClass]
    public class EventControllerTests
    {
        private EventsController _eventsController;
        private Mock<IEventService> _eventServiceMock;
        private Mock<ILogger<EventsController>> _loggerMock;

        [TestInitialize]
        public void Setup()
        {
            _eventServiceMock = new Mock<IEventService>();
            _loggerMock = new Mock<ILogger<EventsController>>();

            _eventsController = new EventsController(
                _eventServiceMock.Object,
                _loggerMock.Object
            );
        }

        [TestMethod]
        public async Task CreateEventTest_()
        {
            //Arrange
            EventDetailsDTO createEvent = new()
            {
                Id = 1,
                Title = "TestTitle",
                Text = "TestText",
                Schedule = "Test",
                ImageUrl = "TestUrl"
            };
            _eventServiceMock.Setup(s => s.CreateEventAsync(It.IsAny<CreateEventDTO>())).ReturnsAsync(createEvent);

            //Act
            var result = await _eventsController.CreateEvent(new CreateEventDTO());
            var okResult = (OkObjectResult)result.Result!;
            var resultValue = (EventDetailsDTO)okResult.Value!;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(createEvent.Id, resultValue.Id);
            Assert.AreEqual(createEvent.Title, resultValue.Title);
            Assert.AreEqual(createEvent.Text, resultValue.Text);
            Assert.AreEqual(createEvent.Schedule, resultValue.Schedule);
            Assert.AreEqual(createEvent.ImageUrl, resultValue.ImageUrl);
        }

        [TestMethod]
        public async Task CreateEventTest_HotelException()
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Image", "Nem támogatott kép formátum") };
            var ex = new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");

            _eventServiceMock.Setup(s => s.CreateEventAsync(It.IsAny<CreateEventDTO>())).Throws(ex);

            //Act
            var result = await _eventsController.CreateEvent(new CreateEventDTO());
            var okResult = (ObjectResult)result.Result!;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(400, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetListOfEventsTest()
        {
            //Arrange
            IEnumerable<EventDetailsDTO> events = new List<EventDetailsDTO>()
        {
                new() {
                     Id = 1,
                Title = "TestTitle",
                Text = "TestText",
                Schedule = "Test",
                ImageUrl = "TestUrl"
                },
                new(){
                    Id = 2,
                Title = "TestTitle2",
                Text = "TestText2",
                Schedule = "Test2",
                ImageUrl = "TestUrl2"
                },
                new(){
                    Id = 3,
                Title = "TestTitle3",
                Text = "TestText3",
                Schedule = "Test3",
                ImageUrl = "TestUrl3"
                },
        };
            _eventServiceMock.Setup(s => s.GetListOfEventsAsync()).ReturnsAsync(events);

            //Act
            var result = await _eventsController.GetListOfEvents();
            var okResult = (OkObjectResult)result.Result!;
            var resultEvents = (List<EventDetailsDTO>)okResult.Value!;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            CollectionAssert.AreEqual(events.ToList(), resultEvents);
        }

        [TestMethod]
        public async Task ModifyEventTest_()
        {
            //Arrange
            EventDetailsDTO modifiedEvent = new()
            {
                Id = 1,
                Title = "TestTitle",
                Text = "TestText",
                Schedule = "Test",
                ImageUrl = "TestUrl"
            };
            _eventServiceMock.Setup(s => s.ModifyEventAsync(It.IsAny<EventModifyDTO>())).ReturnsAsync(modifiedEvent);

            //Act
            var result = await _eventsController.ModifyEvent(new EventModifyDTO());
            var okResult = (OkObjectResult)result.Result!;
            var resultValue = (EventDetailsDTO)okResult.Value!;

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(modifiedEvent.Id, resultValue.Id);
            Assert.AreEqual(modifiedEvent.Title, resultValue.Title);
            Assert.AreEqual(modifiedEvent.Text, resultValue.Text);
            Assert.AreEqual(modifiedEvent.Schedule, resultValue.Schedule);
            Assert.AreEqual(modifiedEvent.ImageUrl, resultValue.ImageUrl);
        }
    }
}
