using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Database;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Room> Rooms { get; set; }

    public DbSet<Image> Images { get; set; }

    public DbSet<Equipment> Equipments { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    public DbSet<Post> Posts { get; set; }

    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

}
