using Hotel.Backend.WebAPI.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Database;

namespace Hotel.Backend.WebAPI.Helpers;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidAudience = configuration["Jwt:Audience"],
                                ValidIssuer = configuration["Jwt:Issuer"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty))
                            };
                        });
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
                  .AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<HotelDbContext>();
        return services;
    }

    public static async Task<WebApplication> UseAuthAsync(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                bool roleExist = await roleManager.RoleExistsAsync(role.ToString());
                if (!roleExist)
                    _ = await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }
        return app;
    }
}
