﻿using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;


namespace HotelBackend.UnitTests.Repositories;

[TestClass]
public class ReservationRepositoryTests
{
    private HotelDbContext _dbContext = null!;
    private ReservationRepository _reservationRepository;

    [TestInitialize]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlite("Data Source = :memory:");
        _dbContext = new HotelDbContext(optionsBuilder.Options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _reservationRepository = new ReservationRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task CreateReservationAsync_AddsReservationToDatabase()
    {
        // Arrange
        var reservation = new Reservation
        {
            BookingFrom = DateTime.Now.AddDays(2),
            BookingTo = DateTime.Now.AddDays(5),
            ApplicationUser = new ApplicationUser { Id = "UserId" },
            Room = new Room { Name = "Bodri" }
        };

        // Act
        var createdReservation = await _reservationRepository.CreateReservationAsync(reservation);

        // Assert
        Assert.AreEqual(reservation, createdReservation);

        var dbReservation = await _dbContext.Reservations.FindAsync(1);
        Assert.AreEqual(reservation, dbReservation);
        Assert.AreEqual(dbReservation.Room.Name, "Bodri");
        Assert.AreEqual(dbReservation.BookingFrom.Day, DateTime.Now.AddDays(2).Day);
    }

    [TestMethod]
    public async Task GetAllReservationsAsync_ReturnsAllReservationsFromDatabase()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new Reservation
            {
                BookingFrom = DateTime.Now.AddDays(2),
                BookingTo = DateTime.Now.AddDays(5),
                ApplicationUser = new ApplicationUser { Id = "UserId1" },
                Room = new Room { Name = "Room1" }
            },
            new Reservation
            {
                BookingFrom = DateTime.Now.AddDays(5),
                BookingTo = DateTime.Now.AddDays(8),
                ApplicationUser = new ApplicationUser { Id = "UserId2" },
                Room = new Room { Name = "Room2" }
            }
        };

        await _dbContext.Reservations.AddRangeAsync(reservations);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _reservationRepository.GetAllReservationsAsync();

        // Assert
        CollectionAssert.AreEqual(reservations, result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("UserId2", result.ElementAt(1).ApplicationUser.Id);

    }
}