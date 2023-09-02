using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBackend.UnitTests.Repositories
{
    [TestClass]
    public class EventRepositoryTests
    {
        private HotelDbContext _dbContext = null!;
        private EventRepository _eventRepository;

        [TestInitialize]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            optionsBuilder.UseSqlite("Data Source = :memory:");
            _dbContext = new HotelDbContext(optionsBuilder.Options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _eventRepository = new EventRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task CreateEventAsync_AddEventToDatabase()
        {
            // Arrange
            var @event = new Event
            {                
                Title = "Event1",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "EventUrlString"
            };

            // Act
            var createdEvent= await _eventRepository.CreateEventAsync(@event);

            // Assert
            Assert.AreEqual(@event, createdEvent);

            var dbEvent = await _dbContext.Events.FindAsync(1);
            Assert.AreEqual(@event, dbEvent);
            Assert.AreEqual(dbEvent.Title, "Event1");
            Assert.AreEqual(dbEvent.Text, "Event Text for test");
            Assert.AreEqual(dbEvent.Schedule, "Hetfo 10.00 - 11.00");
            Assert.AreEqual(dbEvent.ImageUrl, "EventUrlString");
        }

        [TestMethod]
        public async Task GetAllEventsAsync_ReturnsAllEventsFromDatabase()
        {
            // Arrange
            var events = new List<Event> {
            new Event
            {
                Title = "Event1",
                Text = "Event1 Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "Event1UrlString"
            },
            new Event
            {
                Title = "Event2",
                Text = "Event2 Text for test",
                Schedule = "Hetfo 10.30 - 11.30",
                ImageUrl = "Event2UrlString"
            }
            };

            await _dbContext.Events.AddRangeAsync(events);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventRepository.GetAllEventsAsync();

            // Assert
            CollectionAssert.AreEqual(events, result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetEventByIdAsync_ReturnEventByIdFromDatabase()
        {
            // Arrange
            var @event = new Event
            {
                Title = "Event1",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "EventUrlString"
            };
            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();

            // Act
            var createdEvent = _eventRepository.GetEventByIdAsync(1);

            // Assert
            Assert.AreEqual(@event, createdEvent.Result);

            Assert.AreEqual(createdEvent.Result.Title, "Event1");
            Assert.AreEqual(createdEvent.Result.Text, "Event Text for test");
            Assert.AreEqual(createdEvent.Result.Schedule, "Hetfo 10.00 - 11.00");
            Assert.AreEqual(createdEvent.Result.ImageUrl, "EventUrlString");
        }

        [TestMethod]
        public async Task ModifyEventAsync_ReturnModifiedEventFromDatabase()
        {
            // Arrange
            var @event = new Event
            {
                Title = "Event1",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "EventUrlString"
            };
            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();

            // Act
            var modifiedEvent =await _dbContext.Events.FindAsync(1);
            modifiedEvent.Title = "ModifiedTitle";
            await _eventRepository.ModifyEventAsync(modifiedEvent);
            var modifiedEventResult = await _dbContext.Events.FindAsync(1);

            // Assert
            Assert.AreEqual("ModifiedTitle", modifiedEventResult.Title);
            Assert.AreEqual("Event Text for test", modifiedEventResult.Text);
            Assert.AreEqual("Hetfo 10.00 - 11.00", "Hetfo 10.00 - 11.00");
            Assert.AreEqual("EventUrlString", "EventUrlString");
        }


        [TestMethod]
        public async Task DeleteEventAsync_Exist()
        {
            // Arrange
            var @event = new Event
            {
                Title = "Event1",
                Text = "Event Text for test",
                Schedule = "Hetfo 10.00 - 11.00",
                ImageUrl = "EventUrlString"
            };
            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();

            // Act
            var deleteEvent = await _dbContext.Events.FindAsync(1);
            await _eventRepository.DeleteEventAsync(deleteEvent);
            int eventAmount = _dbContext.Events.Count();

            // Assert
            Assert.AreEqual(0, eventAmount);
        }
        }
}
