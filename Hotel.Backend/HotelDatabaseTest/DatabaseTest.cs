using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelDatabaseTest;

[TestClass]
public class DatabaseTest
{
    private HotelDbContext _context;

    [TestInitialize]
    public void SetupInit()
    {
        DbContextOptionsBuilder<HotelDbContext> optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlServer("Data Source=sql6031.site4now.net;Initial Catalog=db_a98a11_hotel;User ID=db_a98a11_hotel_admin;Password=Password123!");
        _context = new HotelDbContext(optionsBuilder.Options);
    }

    [TestMethod]
    public void CreateRoomTest()
    {
        Room roomToCreate = new Room
        {
            Name = "Buksi",
            Price = 32_000M,
            NumberOfBeds = 5,
            Description = "Óriás szoba",
            Available = true,
        };

        _context.Rooms.Add(roomToCreate);
        _context.SaveChanges();

        Room? createdRoom = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");

        Assert.AreEqual("Bodri", createdRoom!.Name);
        Assert.AreEqual(32_000M, createdRoom!.Price);
        Assert.AreEqual(3, createdRoom!.NumberOfBeds);
        Assert.AreEqual("Közepes szoba", createdRoom!.Description);
        Assert.AreEqual(true, createdRoom!.Available);
    }

    [TestMethod]
    public void CreateEquipmentTest()
    {
        Equipment equipmentToCreate = new Equipment
        {
            Name = "WiFi"
        };

        _context.Equipments.Add(equipmentToCreate);
        _context.SaveChanges();

        Equipment createdEquipment = _context.Equipments.FirstOrDefault(equipment => equipment.Name == "WiFi")!;

        Assert.IsNotNull(createdEquipment);
    }

    [TestMethod]
    public void AttachEquipmentToRoomTest()
    {
        Room? room = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");
        Equipment equipment = _context.Equipments.FirstOrDefault(equipment => equipment.Name == "WiFi")!;

        room.Equipments.Add(equipment);
        _context.Rooms.Update(room);
        _context.SaveChanges();

        Room? modifiedRoom = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");

        Assert.AreEqual(1, modifiedRoom.Equipments.Count);
    }

    [TestMethod]
    public void DetachEquipmentFromRoom()
    {
        Room? room = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");
        Equipment equipment = _context.Equipments.FirstOrDefault(equipment => equipment.Name == "WiFi")!;

        room.Equipments.Remove(equipment);
        _context.Rooms.Update(room);
        _context.SaveChanges();

        Room? modifiedRoom = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");

        Assert.AreEqual(0, modifiedRoom.Equipments.Count);

    }

    [TestMethod]
    public void CreateAndAttachImageTest()
    {
        Room? room = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");

        Image imageToAttach1 = new Image
        {
            ImageUrl = "https://www.cesarsway.com/wp-content/uploads/2019/10/AdobeStock_190562703-768x535.jpeg",
            Description = "Kölyök Kutya Fej",
            Room = room
        };
        Image imageToAttach2 = new Image
        {
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/31/Red_Smooth_Saluki.jpg/1200px-Red_Smooth_Saluki.jpg",
            Description = "Agár",
            Room = room
        };

        _context.Images.AddRange(new Image[] { imageToAttach1, imageToAttach2 });
        _context.SaveChanges();

        Assert.AreEqual(2,room.Images.Count);
    }

    [TestMethod]
    public void DetachAndDeleteImageTest()
    {
        Room? room = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");
        Image image = _context.Images.FirstOrDefault(image => image.Description == "Agár");

        room.Images.Remove(image);
        _context.Rooms.Update(room);
        _context.SaveChanges();

        Room? modifiedRoom = _context.Rooms.Include(room => room.Images).FirstOrDefault(room => room.Name == "Bodri");

        Assert.AreEqual(1, modifiedRoom.Images.Count);
    }

    [TestMethod]
    public void DeleteAllImageOfRoomTest()
    {
        Room? room = _context.Rooms.Include(room => room.Images).FirstOrDefault(room => room.Name == "Bodri");

        _context.RemoveRange(room.Images);
        _context.SaveChanges();

        Room? modifiedRoom = _context.Rooms.Include(room => room.Images).FirstOrDefault(room => room.Name == "Bodri");

        Assert.AreEqual(0, modifiedRoom.Images.Count);
    }

    [TestMethod]
    public void CreateReservationTest()
    {
        ApplicationUser? user = _context.Users.FirstOrDefault(user => user.UserName == "Bulisz");
        Room? room = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");

        Reservation reservationToCreate = new Reservation
        {
            Room = room,
            ApplicationUser = user,
            BookingFrom = DateTime.Parse("2023-06-20T16:00"),
            BookingTo = DateTime.Parse("2023-07-11T10:00"),
        };

        _context.Reservations.Add(reservationToCreate);
        _context.SaveChanges();

        Reservation? createdReservation = _context.Reservations.FirstOrDefault(res => res.Room.Name == "Bodri");

        Assert.AreEqual("Bodri", createdReservation.Room.Name);
        Assert.AreEqual("Bulisz", createdReservation.ApplicationUser.UserName);
        Assert.AreEqual("2023. 06. 20. 16:00:00", createdReservation.BookingFrom.ToString());
        Assert.AreEqual("2023. 07. 11. 10:00:00", createdReservation.BookingTo.ToString());
    }

    [TestMethod]
    public void DeleteReservationTest()
    {
        Reservation? reservationToDelete = _context.Reservations.FirstOrDefault(res => res.Room.Name == "Bodri");

        _context.Reservations.Remove(reservationToDelete);
        _context.SaveChanges();

        Reservation? deletedReservation = _context.Reservations.FirstOrDefault(res => res.Room.Name == "Bodri");

        Assert.IsNull(deletedReservation);
    }

    [TestMethod]
    public void DeleteRoomTest()
    {
        Room? roomToDelete = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");
        _context.Rooms.Remove(roomToDelete!);
        _context.SaveChanges();

        Room? deletedRoom = _context.Rooms.FirstOrDefault(room => room.Name == "Bodri");

        Assert.IsNull(deletedRoom);
    }

    [TestMethod]
    public void DeleteEquipmentTest()
    {
        Equipment? equipmentToDelete = _context.Equipments.FirstOrDefault(equipment => equipment.Name == "WiFi");
        _context.Equipments.Remove(equipmentToDelete!);
        _context.SaveChanges();

        Equipment? deletedEquipment = _context.Equipments.FirstOrDefault(equipment => equipment.Name == "WiFi");

        Assert.IsNull(deletedEquipment);
    }

    [TestMethod]
    public void ResetDatabase()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM EquipmentRoom");
        _context.Database.ExecuteSqlRaw("DELETE FROM Equipments");
        _context.Database.ExecuteSqlRaw("DELETE FROM Images");
        _context.Database.ExecuteSqlRaw("DELETE FROM Reservations");
        _context.Database.ExecuteSqlRaw("DELETE FROM Rooms");

        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT (EquipmentRoom, RESEED, 0)");
        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Equipments, RESEED, 0)");
        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Images, RESEED, 0)");
        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Reservations, RESEED, 0)");
        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Rooms, RESEED, 0)");
    }
}