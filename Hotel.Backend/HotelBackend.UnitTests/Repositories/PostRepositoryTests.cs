using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.UnitTests.Repositories;

[TestClass]
public class PostRepositoryTests
{
    private HotelDbContext _dbContext = null!;
    private PostRepository _postRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlite("Data Source = :memory:");
        _dbContext = new HotelDbContext(optionsBuilder.Options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _postRepository = new PostRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task CreatePostAsyncTest()
    {
        //Arrange
        var post = new Post()
        {
            UserName = "User",
            Text = "Test",
            CreatedAt = DateTime.Now,
            Role = Role.Guest,
            Confirmed = false
        };

        //Act
        var createdPost = await _postRepository.CreatePostAsync(post);

        //Assert
        Assert.AreEqual(createdPost.UserName, post.UserName);
    }

    [TestMethod]
    public async Task DeletePostAsyncTest()
    {
        //Arrange
        var post = new Post()
        {
            UserName = "User",
            Text = "Test",
            CreatedAt = DateTime.Now,
            Role = Role.Guest,
            Confirmed = false
        };
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        //Act
        await _postRepository.DeletePostAsync(1);
        var postAmount = _dbContext.Posts.Count();

        //Assert
        Assert.AreEqual(0, postAmount);
    }

    [TestMethod]
    public async Task ConfirmPostAsyncTest()
    {
        //Arrange
        var post = new Post()
        {
            UserName = "User",
            Text = "Test",
            CreatedAt = DateTime.Now,
            Role = Role.Guest,
            Confirmed = false
        };
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        //Act
        await _postRepository.ConfirmPostAsync(1);
        var confirmedPost = await _dbContext.Posts.FirstOrDefaultAsync();

        //Assert
        Assert.AreEqual(confirmedPost!.Confirmed, true);
    }

    [TestMethod]
    public async Task GetConfirmedPostsAsyncTest()
    {
        //Arrange
        var posts = new List<Post>(){
            new Post(){
                        UserName = "User",
                        Text = "Test",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
                        Confirmed = false
                      },
            new Post(){
                        UserName = "User2",
                        Text = "Test2",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
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
        _dbContext.Posts.AddRange(posts);
        await _dbContext.SaveChangesAsync();

        //Act
        var confirmedPosts = await _postRepository.GetConfirmedPostsAsync();

        //Assert
        Assert.AreEqual(2, confirmedPosts.Count());
    }

    [TestMethod]
    public async Task GetNonConfirmedPostsAsyncTest()
    {
        //Arrange
        var posts = new List<Post>(){
            new Post(){
                        UserName = "User",
                        Text = "Test",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
                        Confirmed = false
                      },
            new Post(){
                        UserName = "User2",
                        Text = "Test2",
                        CreatedAt = DateTime.Now,
                        Role = Role.Guest,
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
        _dbContext.Posts.AddRange(posts);
        await _dbContext.SaveChangesAsync();

        //Act
        var confirmedPosts = await _postRepository.GetNonConfirmedPostsAsync();

        //Assert
        Assert.AreEqual(1, confirmedPosts.Count());
    }
}
