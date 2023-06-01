using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Backend.WebAPI.Services;

public class UserCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UserCleanupService> _logger;

    public UserCleanupService(IServiceProvider serviceProvider, ILogger<UserCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var currentTime = DateTime.Now;
                var expiredTime = currentTime.Subtract(TimeSpan.FromMinutes(15));

                List<ApplicationUser> unconfirmedUsers = new List<ApplicationUser>();
                unconfirmedUsers = userManager.Users
                    .Where(user => user.EmailConfirmed == false)
                    .ToList();


                foreach(var user in unconfirmedUsers)
                {
                    
                    
                        if(expiredTime > user.CreatedAt)
                        {
                            _logger.LogInformation(user.LastName, user.FirstName, user.Email, "Felhasználó törölve érvénytelen email-cím miatt");
                            userManager.DeleteAsync(user);
                        }
                    
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(7), stoppingToken);
        }
        
    }
}