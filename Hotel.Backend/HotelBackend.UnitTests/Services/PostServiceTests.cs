using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Moq;

namespace HotelBackend.UnitTests.Services;

[TestClass]
public class PostServiceTests
{
    private PostService _service = null!;
    private readonly Mock<IPostRepository> _postReposítoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();

    [TestInitialize]
    public void Setup()
    {
        _service = new PostService(_mapperMock.Object ,_postReposítoryMock.Object, _dateTimeProviderMock.Object);
    }

    [TestMethod]
    public async Task CreatePostAsyncTest_ToAdmin()
    {
        //Arrange
        var createdPost = new Post
        {
            UserName = "User",
            Text = "Test",
            Role = Role.Admin
        };
        _mapperMock.Setup(m => m.Map<Post>(It.IsAny<PostCreateDTO>())).Returns(createdPost);
        _dateTimeProviderMock.Setup(d => d.Now).Returns(DateTime.Now);
        _postReposítoryMock.Setup(r => r.CreatePostAsync(It.IsAny<Post>())).Returns<Post>(p => Task.FromResult(p));
        _mapperMock.Setup(m => m.Map<PostDetailsDTO>(It.IsAny<Post>())).Returns<Post>(p => new PostDetailsDTO
            {
                Confirmed = p.Confirmed,
                CreatedAt = p.CreatedAt,
                Id = p.Id,
                Role = p.Role.ToString(),
                Text = p.Text,
                UserName = p.UserName,
            });

        //Act
        var post = await _service.CreatePostAsync(new PostCreateDTO());

        //Assert
        Assert.AreEqual(true, post.Confirmed);
        Assert.AreEqual("Admin", post.Role);
        Assert.AreEqual("Test", post.Text);
        Assert.AreEqual("User", post.UserName);
    }

    [TestMethod]
    public async Task CreatePostAsyncTest_ToGuest()
    {
        //Arrange
        var createdPost = new Post
        {
            UserName = "User",
            Text = "Test",
            Role = Role.Guest
        };
        _mapperMock.Setup(m => m.Map<Post>(It.IsAny<PostCreateDTO>())).Returns(createdPost);
        _dateTimeProviderMock.Setup(d => d.Now).Returns(DateTime.Now);
        _postReposítoryMock.Setup(r => r.CreatePostAsync(It.IsAny<Post>())).Returns<Post>(p => Task.FromResult(p));
        _mapperMock.Setup(m => m.Map<PostDetailsDTO>(It.IsAny<Post>())).Returns<Post>(p => new PostDetailsDTO
        {
            Confirmed = p.Confirmed,
            CreatedAt = p.CreatedAt,
            Id = p.Id,
            Role = p.Role.ToString(),
            Text = p.Text,
            UserName = p.UserName,
        });

        //Act
        var post = await _service.CreatePostAsync(new PostCreateDTO());

        //Assert
        Assert.AreEqual(false, post.Confirmed);
        Assert.AreEqual("Guest", post.Role);
        Assert.AreEqual("Test", post.Text);
        Assert.AreEqual("User", post.UserName);
    }

    [TestMethod]
    public void GetConfirmedPostsTest()
    {
        //Arrange
       var posts = new List<Post>(){
            new Post(){
                        UserName = "User",
                        Text = "Test",
                        CreatedAt = DateTime.Now,
                        Role = Role.Admin,
                        Confirmed = true
                      },
            new Post(){
                        UserName = "User2",
                        Text = "Test2",
                        CreatedAt = DateTime.Now,
                        Role = Role.Operator,
                        Confirmed = true
                      },
            new Post(){
                        UserName = "User3",
                        Text = "Test3",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
                        Confirmed = true
                      }
        };
        _postReposítoryMock.Setup(p => p.GetConfirmedPostsAsync()).ReturnsAsync(posts);
        _mapperMock.Setup(m => m.Map<List<PostDetailsDTO>>(It.IsAny<List<Post>>())).Returns<List<PostDetailsDTO>>(posts =>
            posts.Select(p => new PostDetailsDTO
            {
                Confirmed = p.Confirmed,
                CreatedAt = p.CreatedAt,
                Role = p.Role.ToString(),
                UserName = p.UserName,
                Text = p.Text
            }).ToList());

        //Act
        var result = _service.GetConfirmedPostsAsync();

        //Assert
        Assert.AreNotEqual(3, result.Result.Count());
    }

    [TestMethod]
    public void GetNonConfirmedPostsTest()
    {
        //Arrange
        var posts = new List<Post>(){
            new Post(){
                        UserName = "User2",
                        Text = "Test2",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
                        Confirmed = false
                      },
            new Post(){
                        UserName = "User3",
                        Text = "Test3",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
                        Confirmed = true
                      }
        };
        _postReposítoryMock.Setup(p => p.GetNonConfirmedPostsAsync()).ReturnsAsync(posts);
        _mapperMock.Setup(m => m.Map<List<PostDetailsDTO>>(It.IsAny<List<Post>>())).Returns<List<PostDetailsDTO>>(posts =>
            posts.Select(p => new PostDetailsDTO
            {
                Confirmed = p.Confirmed,
                CreatedAt = p.CreatedAt,
                Role = p.Role.ToString(),
                UserName = p.UserName,
                Text = p.Text
            }).ToList());

        //Act
        var result = _service.GetNonConfirmedPostsAsync();

        //Assert
        Assert.AreNotEqual(2, result.Result.Count());
    }
}
