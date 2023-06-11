//using Hotel.Backend.WebAPI.Models.DTO;
//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Net.Http;
//using System.Net.Http.Json;

//namespace HotelBackend.IntegrationTests.Controllers;

//[TestClass]
//public class ReservationsControllerTests
//{
//    private readonly WebApplicationFactory<Program> _factory;
//    private readonly HttpClient _client;

//    public ReservationsControllerTests()
//    {
//        _factory = new WebApplicationFactory<Program>();
//        _client = _factory.CreateClient();
//    }

//    [TestMethod]
//    public async Task ConcurrentReservationsSameRoomSameDate()
//    {

//        // Act
//        string userId1 = "cb122ba4-1265-4c75-9b16-56f0d139cc56";
//        string userId2 = "bcf61e1e-544e-498a-bfd4-382392086f6b";
//        DateTime BookingFrom = DateTime.Today.AddDays(3);
//        DateTime BookingTo = DateTime.Today.AddDays(5);
//        int roomId = 7;

//        // Arrange
//        Task<HttpResponseMessage> task1 = _client.PostAsJsonAsync("hotel/Reservations/CreateReservationForRoom",
//            new ReservationRequestDTO
//            {
//                UserId = userId1,
//                RoomId = roomId,
//                BookingFrom = BookingFrom,
//                BookingTo = BookingTo,
//            });

//        Task<HttpResponseMessage> task2 = _client.PostAsJsonAsync("hotel/Reservations/CreateReservationForRoom",
//            new ReservationRequestDTO
//            {
//                UserId = userId2,
//                RoomId = roomId,
//                BookingFrom = BookingFrom,
//                BookingTo = BookingTo,
//            });

//        HttpResponseMessage[] responses = await Task.WhenAll(task1, task2);


//        // Assert
//       Assert.IsNotNull(responses);
//    }


//}

//internal class CustomWebApplicationFactory<TProgram>
//    : WebApplicationFactory<TProgram> where TProgram : class
//{
//    protected override void ConfigureWebHost(IWebHostBuilder builder)
//    {
//        builder.ConfigureServices(services =>
//        {
//            // Remove potential previous db related services
//            var dbContextDescriptor = services.SingleOrDefault(
//                d => d.ServiceType == typeof(DbContextOptions<HotelDbContext>));

//            if (dbContextDescriptor is not null)
//            {
//                services.Remove(dbContextDescriptor);
//            }

//            var dbConnectionDescriptor = services.SingleOrDefault(
//                d => d.ServiceType == typeof(DbConnection));

//            if (dbConnectionDescriptor is not null)
//            {
//                services.Remove(dbConnectionDescriptor);
//            }

//            // Create open SqliteConnection so EF won't automatically close it.
//            services.AddSingleton<DbConnection>(container =>
//            {
//                var connection = new SqliteConnection("DataSource=:memory:");
//                connection.Open();
//                return connection;
//            });

//            services.AddDbContext<HotelDbContext>((container, options) =>
//            {
//                var connection = container.GetRequiredService<DbConnection>();
//                options.UseSqlite(connection);
//            });
//        });

//        builder.UseEnvironment("Development");
//    }
//}
