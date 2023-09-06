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

    public async Task ConfirmPostAsync(int id)
    {
        Post? post = await _context.Posts.FindAsync(id);

        if (post is not null)
        {
            post.Confirmed = true;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task DeletePostAsync(int id)
    {
        Post? post = await _context.Posts.FindAsync(id);

        if (post is not null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Post>> GetConfirmedPostsAsync()
    {
        return await _context.Posts.Where(p => p.Confirmed == true).OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetNonConfirmedPostsAsync()
    {
        return await _context.Posts.Where(p => p.Confirmed == false).OrderByDescending(p => p.CreatedAt).ToListAsync();
    }
}
