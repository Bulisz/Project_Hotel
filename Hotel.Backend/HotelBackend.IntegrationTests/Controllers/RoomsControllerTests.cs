using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace HotelBackend.IntegrationTests.Controllers;

[TestClass]
public class RoomsControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RoomsControllerTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }


    [TestMethod]
    public async Task GetListOfRoomsTests()
    {
        // Arrange

        // Act
        HttpResponseMessage httpResponseMessage = await _client.GetAsync("hotel/Rooms/GetListOfRooms");
        var listOfRooms = await httpResponseMessage.Content.ReadFromJsonAsync<List<RoomListDTO>>();

        // Assert
        Assert.IsNotNull(listOfRooms);
        Assert.AreEqual(7, listOfRooms.Count);
    }
}