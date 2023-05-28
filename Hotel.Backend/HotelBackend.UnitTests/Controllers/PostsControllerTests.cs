using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class PostsControllerTests
{
    private PostsController _controller;
    private Mock<IPostService> _postServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _postServiceMock = new Mock<IPostService>();
        _controller = new PostsController(_postServiceMock.Object);
    }

    [TestMethod]
    public async Task CreatePostTest()
    {
        //Arrange
        DateTime created = DateTime.Parse("2012-06-12");

        var request = new PostCreateDTO
        {
            Role = "Guest",
            Text = "Jó volt itt",
            UserName = "Geza"
        };

        var response = new PostDetailsDTO
        {
            Id = 1,
            Role = "Guest",
            Text = "Jó volt itt",
            UserName = "Geza",
            CreatedAt = created,
        };

        _postServiceMock.Setup(m => m.CreatePostAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _controller.CreatePost(request);

        //Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<PostDetailsDTO>));
        Assert.AreEqual(response, ((OkObjectResult)result.Result).Value);
    }

    [TestMethod]
    public async Task GetAllPostsTest()
    {
        //Arrange
        DateTime created = DateTime.Parse("2012-06-12");

        List<PostDetailsDTO> list = new List<PostDetailsDTO> {
            new PostDetailsDTO
            {
                Id = 1,
                Role = "Guest",
                Text = "Jó volt itt",
                UserName = "Geza",
                CreatedAt = created,
            },
            new PostDetailsDTO
            {
                Id = 1,
                Role = "Guest",
                Text = "Jó volt ott",
                UserName = "Bulisz",
                CreatedAt = created,
            }
        };

        _postServiceMock.Setup(m => m.GetConfirmedPostsAsync()).ReturnsAsync(list);

        //Act
        var result = await _controller.GetConfirmedPosts();

        //Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<PostDetailsDTO>>));
        var resultOfOk = result.Result;
        Assert.AreEqual(list, ((OkObjectResult)resultOfOk).Value);
    }

}

