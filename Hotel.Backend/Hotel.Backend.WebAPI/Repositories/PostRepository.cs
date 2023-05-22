using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Repositories;

public class PostRepository : IPostRepository
{
    private readonly HotelDbContext _context;

    public PostRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        return await _context.Posts.ToListAsync();
    }
}
