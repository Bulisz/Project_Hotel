using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Database;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

}
