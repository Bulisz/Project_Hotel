using AutoMapper;
using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;

namespace HotelBackend.UnitTests.Services
{
    [TestClass]
    public class EventServiceTests
    {
        
        private EventService _eventService;
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<Cloudinary> _cloudinaryMock;
        private Mock<IMapper> _mapperMock;

        [TestInitialize]
        public void Setup()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _cloudinaryMock = new Mock<Cloudinary>("cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name");
            _mapperMock = new Mock<IMapper>();

            _eventService = new EventService(
                _eventRepositoryMock.Object,
                _cloudinaryMock.Object,
                _mapperMock.Object
            );
        }

        [TestCleanup]
        public void Cleanup() { }

        [TestMethod]
        public async Task GetListOfEventsAsync_ReturnsAllEvents()
        {
            // Arrange
            var events = new List<Event> 
            {
                new Event
            {
                Id = 1,
                Title = "Event1",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name"
            },
            new Event
            {
                Id = 2,
                Title = "Event2",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name"
            },
            new Event
            {
                Id = 3,
                Title = "Event3",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name"
            }
        };
            _eventRepositoryMock.Setup(m => m.GetAllEventsAsync()).ReturnsAsync(events);
            _mapperMock.Setup(m => m.Map<List<EventDetailsDTO>>(It.IsAny<List<Event>>())).Returns<List<Event>>(events =>  events.Select(e => new EventDetailsDTO
            {
                Id=e.Id,
                Title=e.Title,
                Text=e.Text,
                Schedule=e.Schedule,
                ImageUrl=e.ImageUrl
            }).ToList());   
            
            //Act
            var result = await _eventService.GetListOfEventsAsync();    

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(events.Count, result.ToList().Count);

            for (int i = 0; i < events.Count; i++)
            {
                var expectedEvent = events[i];
                var actualEvent = result.ToList()[i];

                Assert.AreEqual(expectedEvent.Id, actualEvent.Id);
                Assert.AreEqual(expectedEvent.Title, actualEvent.Title);
                Assert.AreEqual(expectedEvent.Text, actualEvent.Text);
                Assert.AreEqual(expectedEvent.Schedule, actualEvent.Schedule);
                Assert.AreEqual(expectedEvent.ImageUrl, actualEvent.ImageUrl);
            }
        }

        //[TestMethod]
        //public async Task CreateEventAsync_ValidRequest()
        //{
        //    // Arrange
        //    //Setup mock file using a memory stream
        //    var content = "Hello World from a Fake File";
        //    var fileName = "test.jpg";
        //    var stream = new MemoryStream();
        //    var writer = new StreamWriter(stream);
        //    writer.Write(content);
        //    writer.Flush();
        //    stream.Position = 0;

        //    //create FormFile with desired data
        //    IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        //    var EventDTO = new Event
        //    {
        //        Title = "Event1",
        //        Text = "Event Text for test",
        //        Schedule = "Hetfo 10.00 - 11.00",
        //        ImageUrl = "cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name"
        //    };
        //    var @CreateEventDTO = new CreateEventDTO
        //    {
        //        Title = "Event1",
        //        Text = "Event Text for test",
        //        Schedule = "Hetfo 10.00 - 11.00",
        //        Image = file
        //    };
        //    var uploadParams = new ImageUploadParams
        //    {
        //        File = new FileDescription(@CreateEventDTO.Image?.Name, @CreateEventDTO.Image?.OpenReadStream()),
        //        PublicId = Guid.NewGuid().ToString(),
        //        Folder = "Hotel/Event",
        //    };
        //    _mapperMock.Setup(m => m.Map<Event>(It.IsAny<CreateEventDTO>())).Returns(EventDTO);
        //    _cloudinaryMock.Setup(m => m.UploadAsync(It.IsAny<ImageUploadParams>(), null)).ReturnsAsync(new ImageUploadResult() { SecureUrl = new Uri("cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name") });
        //    //Act
        //    var result = await _eventService.CreateEventAsync(@CreateEventDTO);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.Id);
        //}
    }
    }
