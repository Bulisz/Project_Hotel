using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Database;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    DbSet<Room> Rooms { get; set; }

    DbSet<Image> Images { get; set; }

    DbSet<Equipment> Equipments { get; set; }

    DbSet<Reservation> Reservations { get; set; }
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

}
