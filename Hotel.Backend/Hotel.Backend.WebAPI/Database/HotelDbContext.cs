using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Database;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    DbSet<Room> Rooms;

    DbSet<Image> Images;

    DbSet<Equipment> Equipments;

    DbSet<Reservation> Reservations;
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

}
